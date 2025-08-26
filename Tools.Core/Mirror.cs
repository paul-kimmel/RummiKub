using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Tools.Core
{
	public static class Mirror
	{
		public static T CallPublicMethod<T>(this object o, string methodName, params object[] args)
		{
			return (T)o.GetType().InvokeMember(methodName, BindingFlags.Public |
				BindingFlags.Instance | BindingFlags.InvokeMethod, null, o, args);
		}

		public static T CallNonPublicMethod<T>(this object o, string methodName, params object[] args)
		{
			return (T)o.GetType().InvokeMember(methodName, BindingFlags.NonPublic |
				BindingFlags.Instance | BindingFlags.InvokeMethod, null, o, args);
		}

		public static T CallPublicStaticMethod<T>(this object o, string methodName, params object[] args)
		{
			return (T)o.GetType().InvokeMember(methodName, BindingFlags.Public |
						BindingFlags.Static | BindingFlags.InvokeMethod, null, null, args);
		}

		public static T CallNonPublicStaticMethod<T>(this object o, string methodName, params object[] args)
		{
			return (T)o.GetType().InvokeMember(methodName, BindingFlags.NonPublic |
						BindingFlags.Static | BindingFlags.InvokeMethod, null, null, args);
		}

		public static T CallPublicGetProperty<T>(this object o, string propertyName)
		{
			return (T)o.GetType().InvokeMember(propertyName, BindingFlags.Public |
								BindingFlags.Instance | BindingFlags.GetProperty, null, o, new object[] { });
		}

		public static T CallNonPublicGetProperty<T>(this object o, string propertyName)
		{
			return (T)o.GetType().InvokeMember(propertyName, BindingFlags.NonPublic |
								BindingFlags.Instance | BindingFlags.GetProperty, null, o, new object[] { });
		}

		public static void CallPublicSetProperty(this object o, string propertyName, object value)
		{
			o.GetType().InvokeMember(propertyName, BindingFlags.Public |
				BindingFlags.Instance | BindingFlags.SetProperty, null, o, new object[] { value });
		}

		public static void CallNonPublicSetProperty(this object o, string propertyName, object value)
		{
			o.GetType().InvokeMember(propertyName, BindingFlags.NonPublic |
						BindingFlags.Instance | BindingFlags.SetProperty, null, o, new object[] { value });
		}

		public static T CallPublicStaticGetProperty<T>(this object o, string propertyName)
		{
			return (T)o.GetType().InvokeMember(propertyName, BindingFlags.Public |
								BindingFlags.Static | BindingFlags.GetProperty, null, null, new object[] { });
		}

		public static T CallNonPublicStaticGetProperty<T>(this object o, string propertyName)
		{
			return (T)o.GetType().InvokeMember(propertyName, BindingFlags.NonPublic |
										BindingFlags.Static | BindingFlags.GetProperty, null, null, new object[] { });
		}

		public static void CallPublicStaticSetProperty(this object o, string propertyName, object value)
		{
			o.GetType().InvokeMember(propertyName, BindingFlags.Public |
										BindingFlags.Static | BindingFlags.SetProperty, null, null, new object[] { value });
		}

		public static void CallNonPublicStaticSetProperty(this object o, string propertyName, object value)
		{
			o.GetType().InvokeMember(propertyName, BindingFlags.NonPublic |
										BindingFlags.Static | BindingFlags.SetProperty, null, null, new object[] { value });
		}
	}

}
