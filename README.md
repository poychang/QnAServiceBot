# QnA Service Bot

依照虛擬情境的描述及所要求之技術，達成情境描述的系統

**為了讓每次相同問題，都可以透過Bot方式協助幫助客戶處理，降低客服單位工作負荷。透過問答方式協助客戶解決問題。故需建置一套客服Bot，讓使用者可以與機器人進行問題排除的交談，讓bot協助用戶解決手邊問題，交談介面務必注重使用者體驗，使用者可以用自然語言方式與機器人交談，而非單一字詞或是關鍵字**

## 原始需求

1. 用戶可以在瀏覽器上與Bot進行交談
2. Bot服務還必須可以讓IT人員輸入資料
3. 手機上也可以使用(1)(2)功能

## DEMO

### Web Site

網站連結：[kingstonfe-qna-service.azurewebsites.net](http://kingstonfe-qna-service.azurewebsites.net)
方案簡介：
* `QnAServiceBot.sln` 為此網站的方案
* `AngularWebApp` 資料夾為前端專案
* `QnAServiceBot` 資料夾為後端專案
* `QnAMakerBot` 資料夾為 Bot Service 專案
網站畫面：
![網站 Demo 畫面](https://i.imgur.com/V4ivLgL.jpg)

## Demo App

方案簡介：
* `QnAServiceBot.Mobile.sln` 為此行動裝置 APP 方案
* 使用 Xamarin Forms 框架實作
* 內包含 Android 和 iOS 專案
使用畫面：
![APP 使用畫面](https://i.imgur.com/tJ0Yqri.png)
