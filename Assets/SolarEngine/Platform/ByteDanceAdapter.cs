#if SOLARENGINE_BYTEDANCE
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SolarEngine.MiniGames.info;
using SolarEngine.MiniGames.Utils;
using StarkSDKSpace;
using UnityEngine;

namespace SolarEngine.Platform
{
    public class ByteDanceAdapter : SEAdapterInterface
    {
       
        public SEDeviceInfo setDeviceInfo()
        {
            var sysInfo = StarkSDK.API.GetSystemInfo();
            LogTool.DebugLog("sysInfo" + JsonConvert.SerializeObject(sysInfo));
            string[] tempSys = (sysInfo.system?.Split(' ')) ?? new string[0]; // 最细粒度的系统版本号
            SEDeviceInfo seDeviceInfo = new SEDeviceInfo
            {
                _manufacturer = sysInfo.brand,
                _device_model = sysInfo.model,
                _device_type = AdapterTool.getDeviceType(sysInfo.platform),
                _platform = AdapterTool.getPlatform(sysInfo.platform),
                _os_version = tempSys.Length > 1 ? tempSys[1] : sysInfo.system,
                _screen_height = sysInfo.screenHeight,
                _screen_width = sysInfo.screenWidth,
                _language = sysInfo.language,
                //App 的版本，小程序下为宿主 App 版本号（宿主指微信、抖音、今日头条等）
                _app_version = sysInfo.hostVersion,
                //小程序客户端基础库版本
                _mp_version = sysInfo.sdkVersion,
                _browser_version = sysInfo.hostVersion,
                _app_name = sysInfo.hostName,
                _browser = ""
            };
            return seDeviceInfo;

        }





        public void saveData(string key, object value)
        {
            if (value.GetType() == typeof(int))
            {
                StarkSDK.API.PlayerPrefs.SetInt(key, (int)value);
            }
            else if (value.GetType() == typeof(float))
            {

                StarkSDK.API.PlayerPrefs.SetFloat(key, (float)value);
            }
            else if (value.GetType() == typeof(string))
            {

                StarkSDK.API.PlayerPrefs.SetString(key, (string)value);
            }

            StarkSDK.API.PlayerPrefs.Save();
        }
        public bool  hasKey(string key)
        {
           return StarkSDK.API.PlayerPrefs.HasKey(key);
        }

        public void saveQueue(string str,string data)
        {
            WriteFileStringParam param = new WriteFileStringParam();
            param.filePath =str;
            param.data = data;
            Debug.LogWarning( param.filePath);
            StarkSDK.API.GetStarkFileSystemManager().WriteFile(param);
        }
        public string  getQueue(string str)
        { 
            string filePath =Application.persistentDataPath+"/"+str;
            Debug.LogWarning( filePath);
          return  StarkSDK.API.GetStarkFileSystemManager().ReadFileSync(filePath,"utf8");
        }

        public void clearQueue(string str)
        {
            StarkSDK.API.GetStarkFileSystemManager().RmdirSync(Application.persistentDataPath+"/"+str);
        }
        public object getData(string key, Type type)
        {
            if (!string.IsNullOrEmpty(key) && StarkSDK.API.PlayerPrefs.HasKey(key))
            {

                if (type == typeof(int))
                {
                    return StarkSDK.API.PlayerPrefs.GetInt(key);
                }
                else if (type == typeof(float))
                {
                    return StarkSDK.API.PlayerPrefs.GetFloat(key);
                }
                else if (type == typeof(string))
                {
                    return StarkSDK.API.PlayerPrefs.GetString(key);
                }

                StarkSDK.API.PlayerPrefs.Save();
            }

            return null;
        }

        public void deleteData(string key)
        {
            if (!string.IsNullOrEmpty(key))
            {

                if (StarkSDK.API.PlayerPrefs.HasKey(key))
                {
                    StarkSDK.API.PlayerPrefs.DeleteKey(key);
                }

            }
        }

        public void deleteAll()
        {
            StarkSDK.API.PlayerPrefs.DeleteAll();
        }


        public EnterOptionsInfo getEnterOptionsInfo()
        {
            LaunchOption launchOptionsSync = StarkSDK.API.GetLaunchOptionsSync();

            string scene = launchOptionsSync.Scene;
            Dictionary<string, object> newRefererInfo = new Dictionary<string, object>();
            Dictionary<string, string> RefererInfo = launchOptionsSync.RefererInfo;

#if UNITY_EDITOR
   RefererInfo = new Dictionary<string, string>()
            {
                { "testappId", "123" },
                { "extraData", "rergh" }

            };
#endif
            foreach (var kvp in RefererInfo)
            {
                newRefererInfo.Add(kvp.Key, kvp.Value);
            }

            EnterOptionsInfo enterOptionsInfo = new EnterOptionsInfo
            {
                _launch_scene = (!string.IsNullOrEmpty(scene)) ? scene : "1000",
                _path = !string.IsNullOrEmpty(launchOptionsSync.Path) ? launchOptionsSync.Path : "",

                _query_info = launchOptionsSync.Query,
                _referrer_info = newRefererInfo
            };
            LogTool.DebugLog(JsonConvert.SerializeObject(enterOptionsInfo));
            return enterOptionsInfo;
        }

        public void login(SEAdapterInterface.OnLoginSuccessCallback successCallback,
            SEAdapterInterface.OnLoginFailedCallback failedCallback, bool forceLogin = true)
        {

            StarkSDK.API.GetAccountManager().Login(
                (c1, c2, isLogin) => successCallback?.Invoke(c1, c2, isLogin),
                (errMsg) => failedCallback?.Invoke(errMsg),
                forceLogin
            );

        }

        public void triggerOnShow(SEAdapterInterface.OnShowEvent showEvent)
        {
            StarkSDK.API.GetStarkAppLifeCycle().OnShow += (scene, query, refererInfo) =>
            {
                Dictionary<string, object> RefererInfo = new Dictionary<string, object>();
                foreach (var kvp in refererInfo)
                {
                    RefererInfo.Add(kvp.Key, kvp.Value);
                }

                // 触发 OnShow 事件
                showEvent?.Invoke(scene, query, RefererInfo);
            };

        }

        public void triggerOnHide(SEAdapterInterface.OnHideEvent hideEvent)
        {
            StarkSDK.API.GetStarkAppLifeCycle().OnHide += () =>
            {
                // 触发 OnHide 事件
                hideEvent?.Invoke();
            };
        }

        public string getmptype()
        {
            return "douyin";
        }


   

    }
}
#endif