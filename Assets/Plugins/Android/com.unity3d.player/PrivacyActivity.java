package com.unity3d.player;

import android.app.Activity;
import android.app.AlertDialog;
import android.content.DialogInterface;
import android.content.Intent;
import android.content.SharedPreferences;
import android.os.Bundle;
import android.webkit.WebView;
 
public class PrivacyActivity extends Activity implements DialogInterface.OnClickListener {

   // 隐私协议内容
   final String privacyContext =
     "欢迎您使用Resolution Breach！我们非常重视保护您的个人信息和隐私。您可以通过《Resolution Breach隐私政策》了解我们收集、使用、存储用户个人信息的情况，以及您所享有的相关权利。请您仔细阅读并充分理解相关内容：<br>" +
     "1.为向您提供游戏服务，我们将依据《Resolution Breach隐私政策》收集、使用、存储必要的信息。<br>" +
     "2.基于您的明示授权，虽然我们并没有相关设备权限，您有权拒绝或取消授权。<br>" +
     "3.您可以访问、更正、删除您的个人信息，还可以撤回授权同意、注销账号、投诉举报以及调整其他隐私设置。<br>" +
     "4.我们已采取符合业界标准的安全防护措施保护您的个人信息。<br>" +
     "5.如您是未成年人，请您和您的监护人仔细阅读本隐私政策，并在征得您的监护人授权同意的前提下使用我们的服务或向我们提供个人信息。<br>"+
     "请您阅读完整版<a href=\"https://docs.qq.com/pdf/DRFlSc016Rmpma0Zu?\">《Resolution Breach隐私政策》</a>了解详细信息。<br>"+
     "如您同意<a href=\"https://docs.qq.com/pdf/DRFlSc016Rmpma0Zu?\">《Resolution Breach隐私政策》</a>，请您点击“同意”开始使用我们的产品和服务，我们将尽全力保护您的个人信息安全。<br>";
     
    
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
  
        // 如果已经同意过隐私协议则直接进入Unity Activity
        if (GetPrivacyAccept()){
            EnterUnityActivity();
            return;
        }
        // 弹出隐私协议对话框
        ShowPrivacyDialog();
    }
 
    // 显示隐私协议对话框
    private void ShowPrivacyDialog(){
        WebView webView = new WebView(this);
        webView.loadData(privacyContext, "text/html", "utf-8");         
        AlertDialog.Builder privacyDialog = new AlertDialog.Builder(this);
        privacyDialog.setCancelable(false);
        privacyDialog.setView(webView);
        privacyDialog.setTitle("温馨提示");
        privacyDialog.setPositiveButton("同意",this);
        privacyDialog.setNegativeButton("拒绝",this);
        privacyDialog.create().show();
    }
    
    @Override
    public void onClick(DialogInterface dialogInterface, int i) {
        switch (i){
            case AlertDialog.BUTTON_POSITIVE://点击同意按钮
                SetPrivacyAccept(true);
                EnterUnityActivity(); //启动Unity Activity
                break;
            case AlertDialog.BUTTON_NEGATIVE://点击拒绝按钮,直接退出App
                finish();
                break;
        }
    }
    
    // 启动Unity Activity
    private void EnterUnityActivity(){
        Intent unityAct = new Intent();
        unityAct.setClassName(this, "com.unity3d.player.UnityPlayerActivity");
        this.startActivity(unityAct);
    }
    
    // 本地存储保存同意隐私协议状态
    private void SetPrivacyAccept(boolean accepted){
        SharedPreferences.Editor prefs = this.getSharedPreferences("PlayerPrefs", MODE_PRIVATE).edit();
        prefs.putBoolean("PrivacyAcceptedKey", accepted);
        prefs.apply();
    }
    
    // 获取是否已经同意过
    private boolean GetPrivacyAccept(){
        SharedPreferences prefs = this.getSharedPreferences("PlayerPrefs", MODE_PRIVATE);
        return prefs.getBoolean("PrivacyAcceptedKey", false);
    }
}
