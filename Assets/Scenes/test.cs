// using System;
// using System.Collections.Generic;
// using UnityEngine;
// using System.Text;
// using System.Runtime.InteropServices;
// using Newtonsoft.Json;
// // using SolarEngine.Sample;
// using AOT;
// using SolarEngine.MiniGameRemoteConfig;
// using SolarEngine.MiniGameRemoteConfig.Info;
//
// namespace SolarEngine
// {
//     public class test : MonoBehaviour
//     {
//         private static string className = "com.reyun.se.remote.config.unity.bridge.UnityAndroidSeRemoteConfigManager";
//
//         private static string className2 = "com.reyun.se.remote.config.unity.bridge.UnityAndroidSeRemoteConfigManager"+"test";
//
//         static  AndroidJavaClass IsJavaClassAvailable() {
//             try {
//                 // 尝试创建这个类的实例
//                 var javaClass = new AndroidJavaClass(className2);
//                 Debug.LogError(javaClass);
//                 // 如果成功，类存在
//                 return javaClass;
//             } catch (AndroidJavaException e) {
//                 
//                 var javaClass = new AndroidJavaClass(className);
//                 Debug.LogError("1111"+javaClass);
//                 // 如果抛出异常，类不存在
//                 return javaClass;
//             }
//         }
//         protected static AndroidJavaClass SeRemoteConfigAndroidSDK = new AndroidJavaClass(className);
//         protected static AndroidJavaObject SeRemoteConfigAndroidSDKObject = new AndroidJavaObject("com.reyun.se.remote.config.unity.bridge.UnityAndroidSeRemoteConfigManager");
//
//         private static test _instance;
//
//         public static test Instance
//         {
//             get
//             {
//                 if (!_instance)
//                 {
//                     _instance = FindObjectOfType(typeof(test)) as test;
//                     if (!_instance)
//                     {
//                         GameObject am = new GameObject("test");
//                         _instance = am.AddComponent(typeof(test)) as test;
//                     }
//                 }
//                 return _instance;
//             }
//         }
//        
//    
//      
//         public void SetRemoteDefaultConfig()
//         {
//             Debug.LogError("SESDKSetRemoteDefaultConfig11111"+ IsJavaClassAvailable());
//            
//          //  SESDKSetRemoteDefaultConfig();
//         }
//
//      
//    
//     
//   
//       
//         private void SESDKSetRemoteDefaultConfig()
//         {
//
//         
//
//
//             if (SeRemoteConfigAndroidSDKObject == null)
//             {
//                 Debug.LogError("Android SeRemoteConfigAndroidSDK is null");
//             }
//             else
//             {
//                 SeRemoteConfigAndroidSDKObject.CallStatic("setRemoteDefaultConfig");
//             }
//           
//
//         }
//
//
//     }
// }