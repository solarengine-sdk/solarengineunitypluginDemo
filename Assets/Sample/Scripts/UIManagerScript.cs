/**
 * The MIT License (MIT)
 * 
 * Copyright (c) 2015-Present CG
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using UnityEngine;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

using SolarEngine;
using SolarEngine.MiniGames.info;
using UnityEngine.UI;



public class UIManagerScript : MonoBehaviour
    {
     public Texture2D texture;
        SESDKRemoteConfig remoteConfig = new SESDKRemoteConfig();

      
      
        public  void initRemoteConfig()
        {
      
            Dictionary<string, object> eventProperties = new Dictionary<string, object>
            {
                { "event", "value1" },
                { "k2", 2 }
            };
          
           remoteConfig. SetRemoteConfigEventProperties(eventProperties);  
            Dictionary<string, object> propertiess = new Dictionary<string, object>
            {
                { "user", "value1" },
                { "k2", 2 }
            };
            remoteConfig. SetRemoteConfigUserProperties(propertiess);  
            
            
        }
        
     

        public void setRemoteDefaultConfig()
        {
            #if SOLARENGINE_WECHAT||SOLARENGINE_BYTEDANCE
            Dictionary<string, object> defaultConfig1 =remoteConfig.stringItem("qq", "test");
            Dictionary<string, object> defaultConfig2=remoteConfig.jsonItem("testjson", "{\"test\":\"test\"}");
            Dictionary<string, object> defaultConfig3=remoteConfig.boolItem("testbool", true);
            Dictionary<string, object> defaultConfig4=remoteConfig.intItem("testint", 1);
            
            
            Dictionary<string, object>[] defaultConfigArray  = new Dictionary<string, object>[]{defaultConfig1,defaultConfig2,defaultConfig3,defaultConfig4};
            
            remoteConfig.SetRemoteDefaultConfig(defaultConfigArray);
#endif
        }
     
     
      
   


        public void InitSDK()
        {
       
       
            string appkey = "";
           
           
            Debug.Log("[unity] init click");
            String AppKey = "";   
#if SOLARENGINE_WECHAT
            String AppKey = "";   
#elif SOLARENGINE_BYTEDANCE
            String AppKey = "";   

#endif
        
            MiniGameInitParams  initParams = new MiniGameInitParams();
            initParams.anonymous_openid = "anonymous_openid";
            initParams.unionid = "unionid";
            initParams.openid = "openid";
            SEConfig seConfig = new SEConfig();
#if SOLARENGINE_WECHAT||SOLARENGINE_BYTEDANCE
            seConfig.miniGameInitParams = initParams;
#endif
            RCConfig rcConfig = new RCConfig();
            seConfig.logEnabled = true;
            seConfig.isDebugModel = false;
            rcConfig.mergeType = SERCMergeType.SERCMergeTypeUser;
            rcConfig.enable = true;
            rcConfig.customIDProperties = new Dictionary<string, object> { { "c", 1 } };
            rcConfig.customIDEventProperties = new Dictionary<string, object> { { "e", 1 } };
            rcConfig.customIDUserProperties = new Dictionary<string, object> { { "u", 1 } };
            //
            Analytics.SEAttributionCallback callback = new Analytics.SEAttributionCallback(attributionCallback);
            seConfig.attributionCallback = callback;
            
            Analytics.SESDKInitCompletedCallback initCallback = initSuccessCallback;
            seConfig.initCompletedCallback = initCallback;
            setRemoteDefaultConfig(); 
            initRemoteConfig();
        
           //  
            SolarEngine.Analytics.preInitSeSdk(AppKey);
            SolarEngine.Analytics.initSeSdk(AppKey, seConfig,rcConfig);


        }

  
        public void trackAdClick()
        {
            Debug.Log("[unity] trackAdClick click");
          
            AdClickAttributes AdClickAttributes = new AdClickAttributes();
            AdClickAttributes.ad_platform = "izz";
            AdClickAttributes.mediation_platform = "gromore_test";
            AdClickAttributes.ad_id = "product_id_test";
            AdClickAttributes.ad_type = 1;
            AdClickAttributes.checkId = "123";
            AdClickAttributes.customProperties = getCustomProperties();
            SolarEngine.Analytics.trackAdClick(AdClickAttributes);
          
        }
        public void trackRegister()
        {
            Debug.Log("[unity] trackRegister click");

            RegisterAttributes RegisterAttributes = new RegisterAttributes();
            RegisterAttributes.register_type = "QQ_test";
            RegisterAttributes.register_status = "success_test";
            RegisterAttributes.customProperties = getCustomProperties();
            SolarEngine.Analytics.trackRegister(RegisterAttributes);

            
       
        }  

        public void track()
        {
           
            Debug.Log("[unity] track custom click");

            Dictionary<string, object> customProperties = new Dictionary<string, object>();
            customProperties.Add("event001", 111);
            customProperties.Add("event002", "event002");
            customProperties.Add("event003", 1);

            Dictionary<string, object> preProperties = new Dictionary<string, object>();
            preProperties.Add("_pay_amount", 0.55);
            preProperties.Add("_currency_type", "USD");

           SolarEngine.Analytics.trackCustom("trackCustom",customProperties,preProperties);
            
         
         
        }
        
        
     

          
       

        public void trackAppAtrr() {

            Debug.Log("[unity] trackAppAtrr click");
            AppAttributes AppAttributes = new AppAttributes();
            AppAttributes.ad_network = "toutiao";
            AppAttributes.sub_channel = "103300";
            AppAttributes.ad_account_id = "1655958321988611";
            AppAttributes.ad_account_name = "xxx科技全量18";
            AppAttributes.ad_campaign_id = "1680711982033293";
            AppAttributes.ad_campaign_name = "小鸭快冲计划157-1024";
            AppAttributes.ad_offer_id = "1685219082855528";
            AppAttributes.ad_offer_name = "小鸭快冲单元406-1024";
            AppAttributes.ad_creative_id = "1680128668901378";
            AppAttributes.ad_creative_name = "自动创建20210901178921";
            AppAttributes.ad_creative_name = "自动创建20210901178921";
            AppAttributes.attribution_platform = "se";
            AppAttributes.customProperties = getCustomProperties();
            SolarEngine.Analytics.trackAppAttr(AppAttributes);

        }

        public void trackIAP()
        {
            Debug.Log("[unity] trackIAP click");

            ProductsAttributes productsAttributes = new ProductsAttributes();
            productsAttributes.product_name = "product_name";
            productsAttributes.product_id = "product_id";
            productsAttributes.product_num = 8;
            productsAttributes.currency_type = "CNY";
            productsAttributes.order_id = "order_id";
            productsAttributes.fail_reason = "fail_reason";
            productsAttributes.paystatus = SEConstant_IAP_PayStatus.SEConstant_IAP_PayStatus_success;
            productsAttributes.pay_type = "wechat";
            productsAttributes.pay_amount = 9.9;
            productsAttributes.customProperties = getCustomProperties();
            SolarEngine.Analytics.trackIAP(productsAttributes);
        }

        public void trackAdImpression()
        {
            Debug.Log("[unity] trackAdImpression click");

            AppImpressionAttributes impressionAttributes = new AppImpressionAttributes();
            impressionAttributes.ad_platform = "AdMob";
            //impressionAttributes.ad_appid = "ad_appid";
            impressionAttributes.mediation_platform = "gromore";
            impressionAttributes.ad_id = "product_id";
            impressionAttributes.ad_type = 1;
            impressionAttributes.ad_ecpm = 0.8;
            impressionAttributes.currency_type = "CNY";
            impressionAttributes.is_rendered = true;
            impressionAttributes.customProperties = getCustomProperties();
            SolarEngine.Analytics.trackIAI(impressionAttributes);

        }
 

        public void trackLogin()
        {
            Debug.Log("[unity] trackLogin click");

            LoginAttributes LoginAttributes = new LoginAttributes();
            LoginAttributes.login_type = "QQ_test";
            LoginAttributes.login_status = "success1_test";
            LoginAttributes.customProperties = getCustomProperties();
            SolarEngine.Analytics.trackLogin(LoginAttributes);

        }

        public void trackOrder()
        {
            Debug.Log("[unity] trackOrderclick");

            OrderAttributes OrderAttributes = new OrderAttributes();
            OrderAttributes.order_id = "order_id_test";
            OrderAttributes.pay_amount = 10.5;
            OrderAttributes.currency_type = "CNY";
            OrderAttributes.pay_type = "AIP";
            OrderAttributes.status = "success";
            OrderAttributes.customProperties = getCustomProperties();

            SolarEngine.Analytics.trackOrder(OrderAttributes);

        }
    

        public void userInit()
        {
            Debug.Log("[unity] userInit click");

            Dictionary<string, object> userProperties = new Dictionary<string, object>();
            userProperties.Add("K1", "V1");
            userProperties.Add("K2", "V2");
            userProperties.Add("K3", 2);
            string[] arr = new string[] { "狐狸", "四叶草" };

            userProperties.Add("Kj",arr);
            SolarEngine.Analytics.userInit(userProperties);

        }

        public void userUpdate()
        {
            Debug.Log("[unity] userUpdate click");

            Dictionary<string, object> userProperties = new Dictionary<string, object>();
            userProperties.Add("K1", "V1");
            userProperties.Add("K2", "V2");
            userProperties.Add("K3", 2);
            SolarEngine.Analytics.userUpdate(userProperties);

        }

        public void userAdd()
        {
            Debug.Log("[unity] userAdd click");

            Dictionary<string, object> userProperties = new Dictionary<string, object>();
            userProperties.Add("K1", 10);
            userProperties.Add("K2", 100);
            userProperties.Add("K3", 2);
            SolarEngine.Analytics.userAdd(userProperties);

        }

        public void userUnset()
        {
            Debug.Log("[unity] userUnset click");

            SolarEngine.Analytics.userUnset(new string[] { "K1", "K2" });

        }

        public void userAppend()
        {
            Debug.Log("[unity] userAppend click");

            Dictionary<string, object> userProperties = new Dictionary<string, object>();
            userProperties.Add("K1", "V1");
            userProperties.Add("K2", "V2");
            userProperties.Add("K3", 2);
            SolarEngine.Analytics.userAppend(userProperties);



        }

        public void userDelete()
        {
            Debug.Log("[unity] SEUserDeleteTypeByAccountId click");

            SolarEngine.Analytics.userDelete(SEUserDeleteType.SEUserDeleteTypeByAccountId);
            SolarEngine.Analytics.userDelete(SEUserDeleteType.SEUserDeleteTypeByVisitorId);
        }



        private void initSuccessCallback(int code)
        {
            Debug.Log("SEUnity:initSuccessCallback  code : " + code);

        }


        private void attributionCallback(int code , Dictionary<string, object> attribution)
        {
            Debug.Log("SEUnity: errorCode : " + code);
         
           

        
            if (code == 0)
            {
            foreach (var VARIABLE in attribution)
                {
                Debug.Log("SEUnity: attribution : " + VARIABLE);
                }
            }
            else
            {
                // 归因失败
            }
        }

        private Dictionary<string, object> getCustomProperties() {

            Dictionary<string, object> properties = new Dictionary<string, object>();
            properties.Add("K1", "V1");
            properties.Add("K2", "V2");
            properties.Add("K3", 2);

            return properties;
        }


        private const int Margin = 50;

     
        
        private Vector2 scrollPosition;

        void OnGUI()
        { 
            GUILayout.BeginArea(new Rect(Margin, Margin, Screen.width - 2 * Margin, Screen.height));
            scrollPosition = GUILayout.BeginScrollView(scrollPosition, GUILayout.Width(Screen.width ),
                GUILayout.Height(Screen.height));

            GUIStyle buttonStyle = GUI.skin.button;
            buttonStyle.fontSize = 25;
            buttonStyle.normal.background = texture;
            buttonStyle.normal.textColor = Color.black;
          
            if (GUILayout.Button("InitSDK", GUILayout.Height(80), GUILayout.Width(300))) // 将宽度设置为 150 像素
            {
                InitSDK();
            }
            GUILayout.Space(5);
            if (GUILayout.Button("setSuperProperties", GUILayout.Height(80), GUILayout.Width(300))) // 将宽度设置为 150 像素
            {
                Dictionary<string, object> properties = new Dictionary<string, object>
                {
                    { "Preset1", "这是SuperProperties" },
                    { "propertySuper2", 2 }
                };
          
                Analytics. setSuperProperties(properties);
                
                Dictionary<string, object> propertiess = new Dictionary<string, object>
                {
                    { "Preset1", "test" },
                    { "propertySuper2", 999 }
                };
          
                Analytics. setSuperProperties(propertiess);
            }
            
            
            GUILayout.Space(5);
            
            if (GUILayout.Button("setPresetEvent", GUILayout.Height(80), GUILayout.Width(300))) // 将宽度设置为 150 像素
            {
                Dictionary<string, object> propertiess = new Dictionary<string, object>
                {
                    { "Preset1", "这是Preset" },
                    { "Preset2", 9.99 }
                };
                Analytics. setPresetEvent(SEConstant_Preset_EventType.SEConstant_Preset_EventType_All, propertiess);
                Analytics. setPresetEvent(SEConstant_Preset_EventType.SEConstant_Preset_EventType_AppStart, null);

            }
            
            
            GUILayout.Space(5);
            
            if (GUILayout.Button("login", GUILayout.Height(80), GUILayout.Width(300))) // 将宽度设置为 150 像素
            {
                Analytics.login("12334555");
            }
            
            
            GUILayout.Space(5);
            
            if (GUILayout.Button("logout", GUILayout.Height(80), GUILayout.Width(300))) // 将宽度设置为 150 像素
            {
                Analytics.logout();
            }
            
            
            GUILayout.Space(5);

            if (GUILayout.Button("getAccountId", GUILayout.Height(80), GUILayout.Width(300))) // 将宽度设置为 150 像素
            {
              Debug.Log( Analytics.getAccountId()); 
            }
            
            
            GUILayout.Space(5);
               
            if (GUILayout.Button("setVisitorId", GUILayout.Height(80), GUILayout.Width(300))) // 将宽度设置为 150 像素
            {
                Analytics.setVisitorId("99999999999");
              
            }
            if (GUILayout.Button("getVisitorId", GUILayout.Height(80), GUILayout.Width(300))) // 将宽度设置为 150 像素
            {
                Debug.Log(  Analytics.getVisitorId());
            }
            
            GUILayout.Space(5);
            
            
            
            if (GUILayout.Button("distinct_id", GUILayout.Height(80), GUILayout.Width(300))) // 将宽度设置为 150 像素
            {
#if SOLARENGINE_BYTEDANCE||SOLARENGINE_WECHAT
                Analytics.getDistinct(_distinct);
#endif
            }
            
            GUILayout.Space(5);
            
            if (GUILayout.Button("eventStart", GUILayout.Height(80), GUILayout.Width(300))) // 将宽度设置为 150 像素
            {
                Analytics .eventStart("testEvent");
            }
            
            GUILayout.Space(5);
            
            
            if (GUILayout.Button("eventFinish", GUILayout.Height(80), GUILayout.Width(300))) // 将宽度设置为 150 像素
            {
                Analytics.eventFinish("testEvent",getCustomProperties());
            }
            
            GUILayout.Space(5);
           
            if (GUILayout.Button("SetChannel", GUILayout.Height(80), GUILayout.Width(300))) // 将宽度设置为 150 像素
            {
                Analytics.setChannel("google");
            }
            
            GUILayout.Space(5);

            
            if (GUILayout.Button("unsetSuperProperty", GUILayout.Height(80), GUILayout.Width(300))) // 将宽度设置为 150 像素
            {
                Analytics.unsetSuperProperty("Preset1");
            }
            
            GUILayout.Space(5);
            if (GUILayout.Button("clearSuperProperties", GUILayout.Height(80), GUILayout.Width(300))) // 将宽度设置为 150 像素
            {
                Analytics.clearSuperProperties();
            }
            
            GUILayout.Space(5);
            
            if (GUILayout.Button("reportEventImmediately", GUILayout.Height(80), GUILayout.Width(300))) // 将宽度设置为 150 像素
            {
                Analytics.reportEventImmediately();
            }
            
            GUILayout.Space(5);
            
            if (GUILayout.Button("getAttribution", GUILayout.Height(80), GUILayout.Width(300))) // 将宽度设置为 150 像素
            {
             Dictionary<string,object> dic=Analytics.getAttribution();
             foreach (var VARIABLE in dic)
             {
                 Debug.Log(VARIABLE);
             }
           
            }
            
            GUILayout.Space(5);
            
            if (GUILayout.Button("setChannel", GUILayout.Height(80), GUILayout.Width(300))) // 将宽度设置为 150 像素
            {
              
                Analytics.setChannel(("google"));
           
            }
            
            
            GUILayout.Space(5);
            if (GUILayout.Button("setReferrerTitle", GUILayout.Height(80), GUILayout.Width(300))) // 将宽度设置为 150 像素
            {
#if SOLARENGINE_BYTEDANCE||SOLARENGINE_WECHAT
                  Analytics.setReferrerTitle("setReferrerTitle");
#endif
             
            }
            
            GUILayout.Space(5);
            if (GUILayout.Button("setXcxPageTitle", GUILayout.Height(80), GUILayout.Width(300))) // 将宽度设置为 150 像素
            {
#if SOLARENGINE_BYTEDANCE||SOLARENGINE_WECHAT
                Analytics.setXcxPageTitle("setXcxPageTitle");
#endif
            }       
            GUILayout.Space(5);
            
            if (GUILayout.Button("getPresetProperties", GUILayout.Height(80), GUILayout.Width(300))) // 将宽度设置为 150 像素
            {
              Dictionary<string,object>dic= Analytics.getPresetProperties();
              string str = JsonConvert.SerializeObject(dic);
              Debug.Log(str);
            }       
            GUILayout.Space(5);
            if (GUILayout.Button("fastFetchRemoteConfig", GUILayout.Height(80), GUILayout.Width(300))) // 将宽度设置为 150 像素
            {
                string str=  remoteConfig.FastFetchRemoteConfig("testint");
                Debug.Log(str);
            }       
            GUILayout.Space(5);
            
            
            if (GUILayout.Button("asyncFetchRemoteConfig", GUILayout.Height(80), GUILayout.Width(300))) // 将宽度设置为 150 像素
            {
                Dictionary<string,object> str=   remoteConfig.FastFetchRemoteConfig();
                foreach (var VARIABLE in str)
                {
                    Debug.Log(VARIABLE.Key+" "+VARIABLE.Value);
                }
            }       
            GUILayout.Space(5);
            if (GUILayout.Button("asyncFetchRemoteConfig", GUILayout.Height(80), GUILayout.Width(300))) // 将宽度设置为 150 像素
            {
             
               remoteConfig.AsyncFetchRemoteConfig("testint", onFetchRemoteConfigCallbacks);
            }       
            GUILayout.Space(5);
            
            
            if (GUILayout.Button("asyncFetchRemoteConfig", GUILayout.Height(80), GUILayout.Width(300))) // 将宽度设置为 150 像素
            {
              
               remoteConfig.AsyncFetchRemoteConfig( onFetchRemoteConfigCallback);
              
            
            }       
            GUILayout.Space(5);
            
             GUILayout.EndScrollView();
            GUILayout.EndArea();
        }


        private void onFetchRemoteConfigCallbacks(string result) {
            //异步获取参数下发的回调为string类型，需要开发者根据自己的业务配置进行属性转换，此处以bool类型为例
        
            Debug.Log(result);
        }
        private void onFetchRemoteConfigCallback(Dictionary<string,object> result) 
            //异步获取参数下发的回调为string类型，需要开发者根据自己的业务配置进行属性转换，此处以bool类型为例
            {
             
                string str = JsonConvert.SerializeObject(result);
                Debug.Log(str);
            }
        void _distinct(Distinct distinct)
        {
            Debug.Log(string.Format("distinct_id: {0} \n distinct_id_type: {1}", distinct.distinct_id, distinct.distinct_id_type));

        }
    }
    
    

            
     //   }
    

