using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection.Emit;
using System.Reflection;

namespace AjaxFramework
{
     internal abstract class DynamicMethodHelper
    {
        //该类不能实例化，只能静态调用
        private DynamicMethodHelper() { }

        //通用的可变参数动态方法委托
        public delegate object DynamicMethodDelegate(params object[] paramObjs);

        private static Dictionary<string, DynamicMethodDelegate> cache = new Dictionary<string, DynamicMethodDelegate>();

        private static void LoadIndex(ILGenerator gen, int index)
        {
            switch (index)
            {
                case 0:
                    gen.Emit(OpCodes.Ldc_I4_0);
                    break;
                case 1:
                    gen.Emit(OpCodes.Ldc_I4_1);
                    break;
                case 2:
                    gen.Emit(OpCodes.Ldc_I4_2);
                    break;
                case 3:
                    gen.Emit(OpCodes.Ldc_I4_3);
                    break;
                case 4:
                    gen.Emit(OpCodes.Ldc_I4_4);
                    break;
                case 5:
                    gen.Emit(OpCodes.Ldc_I4_5);
                    break;
                case 6:
                    gen.Emit(OpCodes.Ldc_I4_6);
                    break;
                case 7:
                    gen.Emit(OpCodes.Ldc_I4_7);
                    break;
                case 8:
                    gen.Emit(OpCodes.Ldc_I4_8);
                    break;
                default:
                    if (index < 128)
                    {
                        gen.Emit(OpCodes.Ldc_I4_S, index);
                    }
                    else
                    {
                        gen.Emit(OpCodes.Ldc_I4, index);
                    }
                    break;
            }
        }

        private static void StoreLocal(ILGenerator gen, int index)
        {
            switch (index)
            {
                case 0:
                    gen.Emit(OpCodes.Stloc_0);
                    break;
                case 1:
                    gen.Emit(OpCodes.Stloc_1);
                    break;
                case 2:
                    gen.Emit(OpCodes.Stloc_2);
                    break;
                case 3:
                    gen.Emit(OpCodes.Stloc_3);
                    break;
                default:
                    if (index < 128)
                    {
                        gen.Emit(OpCodes.Stloc_S, index);
                    }
                    else
                    {
                        gen.Emit(OpCodes.Stloc, index);
                    }
                    break;
            }
        }

        private static void LoadLocal(ILGenerator gen, int index)
        {
            switch (index)
            {
                case 0:
                    gen.Emit(OpCodes.Ldloc_0);
                    break;
                case 1:
                    gen.Emit(OpCodes.Ldloc_1);
                    break;
                case 2:
                    gen.Emit(OpCodes.Ldloc_2);
                    break;
                case 3:
                    gen.Emit(OpCodes.Ldloc_3);
                    break;
                default:
                    if (index < 128)
                    {
                        gen.Emit(OpCodes.Ldloc_S, index);
                    }
                    else
                    {
                        gen.Emit(OpCodes.Ldloc, index);
                    }
                    break;
            }
        }

        public static DynamicMethodDelegate GetDynamicMethodDelegate(MethodInfo genericMethodInfo,
            params Type[] genericParameterTypes)
        {
            #region 检查参数的有效性

            if (genericMethodInfo == null)
            {
                throw new ArgumentNullException("需要被调用的方法的genericMethodInfo不能为空!");
            }

            if (genericParameterTypes != null)
            {
                if (genericParameterTypes.Length != genericMethodInfo.GetGenericArguments().Length)
                {
                    throw new ArgumentException("genericMethodInfo和泛型参数类型的数量不一致!");
                }
            }
            else
            {
                if (genericMethodInfo.GetGenericArguments().Length > 0)
                {
                    throw new ArgumentException("没有为genericMethodInfo指定泛型参数类型!");
                }
            }

            #endregion

            #region  构造用于缓存的key

            string key = genericMethodInfo.DeclaringType.ToString() + "|" + genericMethodInfo.ToString();
            if (genericParameterTypes != null)
            {
                for (int i = 0; i < genericParameterTypes.Length; ++i)
                {
                    key += "|" + genericParameterTypes[i].ToString();
                }
            }

            #endregion

            DynamicMethodDelegate dmd;

            lock (cache)
            {
                if (cache.ContainsKey(key))
                {
                    dmd = cache[key];
                }
                else
                {
                    //动态创建一个封装了泛型方法调用的非泛型方法，并返回绑定到他的DynamicMethodDelegate的实例
                    //返回的动态方法的实现在编译期就是以显式方法调用泛型方法的，因此，最大程度上避免了反射的性能损失
                    DynamicMethod dm = new DynamicMethod(Guid.NewGuid().ToString("N"),
                        typeof(object),
                        new Type[] { typeof(object[]) },
                        typeof(string).Module);

                    ILGenerator il = dm.GetILGenerator();

                    #region 创建所有方法的参数的本地变量

                    //首先获得目标方法的MethodInfo
                    MethodInfo makeGenericMethodInfo;
                    if (genericParameterTypes != null && genericParameterTypes.Length > 0)
                    {
                        makeGenericMethodInfo = genericMethodInfo.MakeGenericMethod(genericParameterTypes);
                    }
                    else
                    {
                        makeGenericMethodInfo = genericMethodInfo;
                    }

                    //声明本地变量
                    ParameterInfo[] pis = makeGenericMethodInfo.GetParameters();
                    for (int i = 0; i < pis.Length; ++i)
                    {
                        il.DeclareLocal(pis[i].ParameterType);
                    }

                    #endregion

                    #region 从paramObjs参数中解析所有参数值到本地变量中

                    for (int i = 0; i < pis.Length; ++i)
                    {
                        il.Emit(OpCodes.Ldarg_0);
                        LoadIndex(il, i);
                        il.Emit(OpCodes.Ldelem_Ref);
                        if (pis[i].ParameterType.IsValueType)
                        {
                            il.Emit(OpCodes.Unbox_Any, pis[i].ParameterType);
                        }
                        else if (pis[i].ParameterType != typeof(object))
                        {
                            il.Emit(OpCodes.Castclass, pis[i].ParameterType);
                        }
                        StoreLocal(il, i);
                    }

                    #endregion

                    #region 执行目标方法

                    for (int i = 0; i < pis.Length; ++i)
                    {
                        LoadLocal(il, i);
                    }
                    if (makeGenericMethodInfo.IsStatic)
                    {
                        il.Emit(OpCodes.Call, makeGenericMethodInfo);
                    }
                    else
                    {
                        throw new NotImplementedException("暂时还没有实现该功能!");
                    }

                    if (makeGenericMethodInfo.ReturnType == typeof(void))
                    {
                        il.Emit(OpCodes.Ldnull);
                    }

                    #endregion

                    il.Emit(OpCodes.Ret);

                    dmd = (DynamicMethodDelegate)dm.CreateDelegate(typeof(DynamicMethodDelegate));
                    cache.Add(key, dmd);
                }
            }

            return dmd;
        }
    }
}
