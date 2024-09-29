using System;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using System.Runtime.InteropServices;
using Newtonsoft.Json;
// using SolarEngine.Sample;
using AOT;
using SolarEngine.MiniGameRemoteConfig;
using SolarEngine.MiniGameRemoteConfig.Info;

namespace SolarEngine
{
    public class SESDKRemoteConfig : MonoBehaviour
    {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Initializes()
        {
            [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
            static void Initializes()
            {
#if SOLARENGINE_WECHAT
            SEAdapterInterface _adapter = new SolarEngine.Platform. WeChatAdapter();
                MiniRemoteConfigInfo.setAdapterInterface(_adapter);
#elif SOLARENGINE_BYTEDANCE
                SEAdapterInterface _adapter = new SolarEngine.Platform.ByteDanceAdapter();
                
                MiniRemoteConfigInfo .setAdapterInterface(_adapter);
#endif
            }
        }
#if UNITY_ANDROID && !UNITY_EDITOR&&!SOLARENGINE_BYTEDANCE

        protected static AndroidJavaClass SeRemoteConfigAndroidSDK = new AndroidJavaClass("com.reyun.se.remote.config.unity.bridge.UnityAndroidSeRemoteConfigManager");
        protected static AndroidJavaObject SeRemoteConfigAndroidSDKObject = new AndroidJavaObject("com.reyun.se.remote.config.unity.bridge.UnityAndroidSeRemoteConfigManager");

#endif
        private FetchRemoteConfigCallback fetchRemoteConfigCallback_private = null;
        public delegate void FetchRemoteConfigCallback(string result);

        private FetchAllRemoteConfigCallback fetchAllRemoteConfigCallback_private = null;
        public delegate void FetchAllRemoteConfigCallback(Dictionary<string, object> result);

        private static List<Action> waitingTaskList = new List<Action>();
        private static List<Action> executingTaskList = new List<Action>();

        private static SESDKRemoteConfig _instance = null;
#if SOLARENGINE_BYTEDANCE||SOLARENGINE_BYTEDANCE||SOLARENGINE_WECHAT
        private void init(SERemoteConfigInterface se)
        {
            MiniRemoteConfigWrapper.Instance.init(se);
               
        }
        #region 新增set方法
        
        public Dictionary<string, object> stringItem(string name, string value)
        {
        return  MiniRemoteConfigWrapper.Instance.stringItem(name, value);
        }

        public Dictionary<string, object> intItem(string name, int value)
        {
            return  MiniRemoteConfigWrapper.Instance.intItem(name, value);

        }


        public Dictionary<string, object> boolItem(string name, bool value)
        {
            return  MiniRemoteConfigWrapper.Instance.boolItem(name, value);

        }

        public Dictionary<string, object> jsonItem(string name, string value)
        {
            return  MiniRemoteConfigWrapper.Instance.jsonItem(name, value);

        }

        #endregion
#endif
       
        public static SESDKRemoteConfig Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = FindObjectOfType(typeof(SESDKRemoteConfig)) as SESDKRemoteConfig;
                    if (!_instance)
                    {
                        GameObject am = new GameObject("SESDKRemoteConfig");
                        _instance = am.AddComponent(typeof(SESDKRemoteConfig)) as SESDKRemoteConfig;
                    }
                }
                return _instance;
            }
        }

        public static void PostTask(Action task)
        {
            lock (waitingTaskList)
            {
                waitingTaskList.Add(task);
            }
        }

        private void Update()
        {
            lock (waitingTaskList)
            {
                if (waitingTaskList.Count > 0)
                {
                    executingTaskList.AddRange(waitingTaskList);

                    waitingTaskList.Clear();
                }
            }

            for (int i = 0; i < executingTaskList.Count; ++i)
            {
                Action task = executingTaskList[i];
                try
                {
                    task();
                }
                catch (Exception e)
                {
                    Debug.LogError(e.Message, this);
                }
            }

            executingTaskList.Clear();
        }

        /// <summary>
        /// 设置默认配置，在线参数SDK需要开发者预置一份默认配置到用户app中，方便在线参数SDK使用此默认配置进行兜底操作。
        /// </summary>
        /// <param name="defaultConfig">默认配置/param>
        public void SetRemoteDefaultConfig(Dictionary<string, object>[] defaultConfig)
        {
            SESDKSetRemoteDefaultConfig(defaultConfig);
        }

        /// <summary>
        /// 设置自定义事件属性，请求参数配置时后端会使用该属性匹配
        /// </summary>
        /// <param name="properties">跟在后台页面设置的事件属性对应</param>
        public void SetRemoteConfigEventProperties(Dictionary<string, object> properties)
        {
            SESDKSetRemoteConfigEventProperties(properties);
        }

        /// <summary>
        /// 设置自定义用户属性，请求参数配置时后端会使用该属性匹配
        /// </summary>
        /// <param name="properties">跟在后台页面设置的用户属性对应</param>
        public void SetRemoteConfigUserProperties(Dictionary<string, object> properties)
        {
            SESDKSetRemoteConfigUserProperties(properties);
        }

        /// <summary>
        /// 同步获取参数配置
        /// 优先从缓存配置查询，查询不到则从默认配置查询，都查询不到则返回nil
        /// </summary>
        /// <param name="key">在后台页面设置的参数key，命中则返回对应值value</param>
        public string FastFetchRemoteConfig(string key)
        {
            return SESDKFastFetchRemoteConfig(key);
        }

        /// <summary>
        /// 同步获取所有参数配置
        /// 优先从缓存配置查询，查询不到则从默认配置查询，都查询不到则返回nil
        /// <returns>Dictionary 字典，代表所有参数配置</returns>
        /// </summary>
        public Dictionary<string, object> FastFetchRemoteConfig()
        {
            return SESDKFastFetchAllRemoteConfig();
        }

        /// <summary>
        /// 异步获取参数配置，回调方法：OnRemoteConfigFetchCompletion
        /// 请求服务端配置后与本地缓存配置合并，然后从缓存配置查询，查询不到则从默认配置中查询，都查询不到则返回nil 
        /// </summary>
        /// <param name="key">在后台页面设置的参数key，命中则返回对应值value</param>
        /// <param name="callback">参数配置异步回调</param>
        public void AsyncFetchRemoteConfig(string key, FetchRemoteConfigCallback callback)
        {
#if SOLARENGINE_BYTEDANCE||SOLARENGINE_WECHAT
            
            MiniRemoteConfigWrapper.MiniFetchRemoteConfigCallback _miniFetch=  (MiniRemoteConfigWrapper.MiniFetchRemoteConfigCallback)Delegate.CreateDelegate(typeof(MiniRemoteConfigWrapper.MiniFetchRemoteConfigCallback), callback.Target, callback.Method);
          
            MiniRemoteConfigWrapper.Instance.asyncFetchRemoteConfig(key,_miniFetch);
            return;
#endif
            SESDKRemoteConfig.Instance.fetchRemoteConfigCallback_private = callback;
            SESDKAsyncFetchRemoteConfig(key);
        }

        /// <summary>
        /// 异步获取所有参数配置
        /// 请求服务端配置后与本地缓存配置合并，然后从缓存配置查询，查询不到则从默认配置中查询，都查询不到则返回nil 
        /// </summary>
        /// <param name="callback">参数配置异步回调</param>
        public void AsyncFetchRemoteConfig(FetchAllRemoteConfigCallback callback)
        {
          
#if SOLARENGINE_BYTEDANCE||SOLARENGINE_WECHAT
            
           
            MiniRemoteConfigWrapper.MiniFetchAllRemoteConfigCallback _miniFetchAll=  (MiniRemoteConfigWrapper.MiniFetchAllRemoteConfigCallback)Delegate.CreateDelegate(typeof(MiniRemoteConfigWrapper.MiniFetchAllRemoteConfigCallback), callback.Target, callback.Method);
          
            MiniRemoteConfigWrapper.Instance.asyncFetchRemoteConfig(_miniFetchAll);
            return;
#endif
            SESDKRemoteConfig.Instance.fetchAllRemoteConfigCallback_private = callback;
            SESDKAsyncFetchAllRemoteConfig();
        }

        private void SESDKSetRemoteDefaultConfig(Dictionary<string, object>[] defaultConfig)
        {

            if (defaultConfig == null)
            {
                return;
            }
#if SOLARENGINE_BYTEDANCE||SOLARENGINE_WECHAT
           MiniRemoteConfigWrapper.Instance.setRemoteDefaultConfig(defaultConfig);
           return;
#endif
            string defaultConfigJSONString = JsonConvert.SerializeObject(defaultConfig);

#if UNITY_EDITOR
            Debug.Log("Unity Editor: SESDKSetRemoteConfigEventProperties error");
#elif UNITY_ANDROID&&!SOLARENGINE_BYTEDANCE
            SeRemoteConfigAndroidSDK.CallStatic("setRemoteDefaultConfig",defaultConfigJSONString);
#elif (UNITY_5 && UNITY_IOS) || UNITY_IPHONE
            __iOSSESDKSetDefaultConfig(defaultConfigJSONString);
#else

#endif

        }


        private void SESDKSetRemoteConfigEventProperties(Dictionary<string, object> properties)
        {

            if (properties == null)
            {
                return;
            }
#if SOLARENGINE_BYTEDANCE||SOLARENGINE_WECHAT
            MiniRemoteConfigWrapper.Instance.setRemoteConfigEventProperties(properties);
            return;
#endif
            string propertiesJSONString = JsonConvert.SerializeObject(properties);

#if UNITY_EDITOR
            Debug.Log("Unity Editor: SESDKSetRemoteConfigEventProperties error");
#elif UNITY_ANDROID&&!SOLARENGINE_BYTEDANCE
            SeRemoteConfigAndroidSDK.CallStatic("setRemoteConfigEventProperties",propertiesJSONString);
#elif (UNITY_5 && UNITY_IOS) || UNITY_IPHONE
            __iOSSESDKSetRemoteConfigEventProperties(propertiesJSONString);
#else

#endif

        }

        private void SESDKSetRemoteConfigUserProperties(Dictionary<string, object> properties)
        {

            if (properties == null)
            {
                return;
            }
#if SOLARENGINE_BYTEDANCE||SOLARENGINE_WECHAT
            MiniRemoteConfigWrapper.Instance.setRemoteConfigUserProperties(properties);
            return;
#endif
            string propertiesJSONString = JsonConvert.SerializeObject(properties);


#if UNITY_EDITOR
            Debug.Log("Unity Editor: SESDKSetRemoteConfigEventProperties error");
#elif UNITY_ANDROID&&!SOLARENGINE_BYTEDANCE
            SeRemoteConfigAndroidSDK.CallStatic("setRemoteConfigUserProperties",propertiesJSONString);
#elif (UNITY_5 && UNITY_IOS) || UNITY_IPHONE
            __iOSSESDKSetRemoteConfigUserProperties(propertiesJSONString);
#else

#endif

        }

       
        private string SESDKFastFetchRemoteConfig(string key)
        {
            if (key == null)
            {
                return null;
            }
#if SOLARENGINE_BYTEDANCE||SOLARENGINE_WECHAT
       
            return MiniRemoteConfigWrapper.Instance.fastFetchRemoteConfig(key);
#endif
#if UNITY_EDITOR
            Debug.Log("Unity Editor: SESDKFastFetchRemoteConfig error");
            return null;
#elif UNITY_ANDROID&&!SOLARENGINE_BYTEDANCE
            string result = SeRemoteConfigAndroidSDK.CallStatic<string>("fastFetchRemoteConfig",key);
            return result;
#elif (UNITY_5 && UNITY_IOS) || UNITY_IPHONE
            return __iOSSESDKFastFetchRemoteConfig(key);
#else
            return null;
#endif
        }

        private Dictionary<string, object> SESDKFastFetchAllRemoteConfig()
        {
#if SOLARENGINE_BYTEDANCE||SOLARENGINE_WECHAT
       
            return MiniRemoteConfigWrapper.Instance.fastFetchRemoteConfig();
#endif
#if UNITY_EDITOR
            Debug.Log("Unity Editor: SESDKFastFetchRemoteConfig error");
            return null;
#elif UNITY_ANDROID&&!SOLARENGINE_BYTEDANCE
            string result = SeRemoteConfigAndroidSDK.CallStatic<string>("fastFetchRemoteConfig");
            try{
                return JsonConvert.DeserializeObject<Dictionary<string, object>>(result);
            }catch(Exception e){

            }
            return null;
#elif (UNITY_5 && UNITY_IOS) || UNITY_IPHONE
            string result = __iOSSESDKFastFetchAllRemoteConfig();
            try{
                return JsonConvert.DeserializeObject<Dictionary<string, object>>(result);
            }catch(Exception e){

            }
            return null;
#else
        return null;
#endif
        }

        private void SESDKAsyncFetchAllRemoteConfig()
        {

#if UNITY_EDITOR
            Debug.Log("Unity Editor: SESDKAsyncFetchAllRemoteConfig error");
#elif UNITY_ANDROID&&!SOLARENGINE_BYTEDANCE
             SeRemoteConfigAndroidSDK.CallStatic("asyncFetchRemoteConfig",new OnRemoteConfigReceivedAllData());
#elif (UNITY_5 && UNITY_IOS) || UNITY_IPHONE
            __iOSSESDKAsyncFetchAllRemoteConfig(OnRemoteConfigiOSReceivedAllData);
#else

#endif
        }

        private void SESDKAsyncFetchRemoteConfig(string key)
        {
            if (key == null)
            {
                // TODO callback
                return;
            }

#if UNITY_EDITOR
            Debug.Log("Unity Editor: SESDKAsyncFetchRemoteConfig error");
#elif UNITY_ANDROID&&!SOLARENGINE_BYTEDANCE
             SeRemoteConfigAndroidSDK.CallStatic("asyncFetchRemoteConfig",key,new OnRemoteConfigReceivedData());
#elif (UNITY_5 && UNITY_IOS) || UNITY_IPHONE
            __iOSSESDKAsyncFetchRemoteConfig(key,OnRemoteConfigiOSReceivedData);
#else

#endif
        }


#if UNITY_IPHONE
     //回调函数，必须MonoPInvokeCallback并且是static
    [MonoPInvokeCallback(typeof(FetchRemoteConfigCallback))]
    private static void OnRemoteConfigiOSReceivedData(string result)
    {
        OnFetchRemoteConfigCallback(result);
    }

    //回调函数，必须MonoPInvokeCallback并且是static
    [MonoPInvokeCallback(typeof(FetchRemoteConfigCallback))]
    private static void OnRemoteConfigiOSReceivedAllData(string result)
    {

            Dictionary<string, object> dict = null;
            if (result != null) { 
                dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(result);
            }
            OnFetchAllRemoteConfigCallback(dict);

    }
#endif


#if UNITY_ANDROID && !UNITY_EDITOR&&!SOLARENGINE_BYTEDANCE
    private sealed class OnRemoteConfigReceivedData: AndroidJavaProxy
    {
        public OnRemoteConfigReceivedData():base("com.reyun.se.remote.config.unity.bridge.OnRemoteConfigReceivedDataForUnity")
        {
            
        }
        public void onResultForUnity(String result)
        {
            OnFetchRemoteConfigCallback(result);
        }
    }
#endif

        private static void OnFetchRemoteConfigCallback(String result)
    {

        SESDKRemoteConfig.PostTask(() =>
        {
            if (SESDKRemoteConfig.Instance.fetchRemoteConfigCallback_private != null)
            {
                SESDKRemoteConfig.Instance.fetchRemoteConfigCallback_private.Invoke(result);
            }
        });

    }

#if UNITY_ANDROID && !UNITY_EDITOR&&!SOLARENGINE_BYTEDANCE
    private sealed class OnRemoteConfigReceivedAllData: AndroidJavaProxy
    {
        public OnRemoteConfigReceivedAllData():base("com.reyun.se.remote.config.unity.bridge.OnRemoteConfigReceivedAllDataForUnity")
        {
            
        }
        public void onResultForUnity(String result)
        {
            Dictionary<string, object> dict = JsonConvert.DeserializeObject<Dictionary<string, object>>(result);
            if(dict != null){
                OnFetchAllRemoteConfigCallback(dict);
            }
        }
    }
#endif

    private static void OnFetchAllRemoteConfigCallback(Dictionary<string, object> result)
    {

        SESDKRemoteConfig.PostTask(() =>
        {
            if (SESDKRemoteConfig.Instance.fetchAllRemoteConfigCallback_private != null)
            {
                SESDKRemoteConfig.Instance.fetchAllRemoteConfigCallback_private.Invoke(result);
            }
        });

    }



#if UNITY_IPHONE



        [DllImport("__Internal")]
        private static extern void __iOSSESDKSetDefaultConfig(string config);

        [DllImport("__Internal")]
        private static extern void __iOSSESDKSetRemoteConfigEventProperties(string properties);

        [DllImport("__Internal")]
        private static extern void __iOSSESDKSetRemoteConfigUserProperties(string properties);

        [DllImport("__Internal")]
        private static extern string __iOSSESDKFastFetchRemoteConfig(string key);

        [DllImport("__Internal")]
        private static extern string __iOSSESDKFastFetchAllRemoteConfig();

        [DllImport("__Internal")]
        private static extern void __iOSSESDKAsyncFetchRemoteConfig(string key, FetchRemoteConfigCallback callBack);

        [DllImport("__Internal")]
        private static extern void __iOSSESDKAsyncFetchAllRemoteConfig(FetchRemoteConfigCallback callBack);

#endif

    }
}