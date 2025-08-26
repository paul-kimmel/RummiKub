using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{
	public class ProxyClass<T> : System.Dynamic.DynamicObject
	{
		private readonly T proxy = default(T);
		
		public ProxyClass(T proxy)
		{
			this.proxy = proxy;
		}

		public override bool TrySetMember(SetMemberBinder binder, object value)
		{
			if (members.ContainsKey(binder.Name))
			{
				Invoke(binder.Name, value);
				return true;
			}

			try
			{
				var o = GetMember(binder.Name);
				if (o is FieldInfo)
				{
					(o as FieldInfo).SetValue(proxy, value);
					return true;
				}
				else if (o is PropertyInfo)
				{
					(o as PropertyInfo).SetValue(proxy, value);
					return true;
				}
				return false;
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
				return false;
			}

		}

		public override bool TryGetMember(GetMemberBinder binder, out object result)
		{
			if (members.ContainsKey(binder.Name))
			{
				result = Invoke(binder.Name);
				return true;
			}

			try
			{
				var o = GetMember(binder.Name);
				if (o is FieldInfo)
				{
					result = (o as FieldInfo).GetValue(proxy);
					return true;
				}
				else if (o is PropertyInfo)
				{
					result = (o as PropertyInfo).GetValue(proxy);
					return true;
				}
				result = null;
				return false;
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
				result = null;
				return false;
			}
		}

		private object Invoke(string name, params object[] args)
		{
			LambdaExpression o = (LambdaExpression)members[name];
			try
			{
				return o.Compile().DynamicInvoke(args);
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
				return null;
			}
		}

		public static FieldInfo GetField(Type type, string name)
		{
			return type.GetField(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
		}

		public static PropertyInfo GetProperty(Type type, string name)
		{
			return typeof(T).GetProperty(name, BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
		}


		private MemberInfo GetMember(string name)
		{
			return typeof(T).GetMember(name, MemberTypes.All,
				BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public)[0];
		}

		private MethodInfo GetMethod(string name, object[] args)
		{
			List<Type> types = new List<Type>();
			foreach (var o in args)
			{
				try
				{
					types.Add(o.GetType());
				}
				catch(NullReferenceException ex)
				{
					types.Add(typeof(Nullable));
					Debug.WriteLine(ex.Message);
				}
			}

			//BUGFIX 3457: This will fail if you pass NULL for nullable arguments.
			return typeof(T).GetMethod(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance 
				| BindingFlags.Static, null, types.ToArray(), null);
		}

		public override bool TryInvokeMember(InvokeMemberBinder binder, object[] args, out object result)
		{
			if (members.ContainsKey(binder.Name))
			{
				result = Invoke(binder.Name, args);
				return true;
			}

			try
			{
				var o = GetMethod(binder.Name, args);
				if (o != null)
				{
					result = (o as MethodInfo).Invoke(proxy, args);
					return true;
				}

				result = null;
				return false;
			}
			catch (Exception ex)
			{
				Debug.WriteLine(ex.Message);
				result = null;
				return false;
			}
		}



		private Hashtable members = new Hashtable();

		public void Clear()
		{
			members.Clear();
		}

		public void Add<T1>(string key, Func<T1> func)
		{
			members.Add(key, func);
		}

		public void Add<T1, T2>(string key, Func<T1, T2> func)
		{
			members.Add(key, func);
		}

		public void Add<T1, T2, T3>(string key, Func<T1, T2, T3> func)
		{
			members.Add(key, func);
		}

		public void Add<T1, T2, T3, T4>(string key, Func<T1, T2, T3, T4> func)
		{
			members.Add(key, func);
		}

		public void Add<T1, T2, T3, T4, T5>(string key, Func<T1, T2, T3, T4, T5> func)
		{
			members.Add(key, func);
		}

		public void Add<T1, T2, T3, T4, T5, T6>(string key, Func<T1, T2, T3, T4, T5, T6> func)
		{
			members.Add(key, func);
		}

		public void Add<T1, T2, T3, T4, T5, T6, T7>(string key, Func<T1, T2, T3, T4, T5, T6, T7> func)
		{
			members.Add(key, func);
		}

		public void Add<T1, T2, T3, T4, T5, T6, T7, T8>(string key, Func<T1, T2, T3, T4, T5, T6, T7, T8> func)
		{
			members.Add(key, func);
		}


		public void Add<T1>(string key, Action<T1> action)
		{
			members.Add(key, action);
		}


		public void Add<T1, T2>(string key, Action<T1, T2> action)
		{
			members.Add(key, action);
		}

		public void Add<T1, T2, T3>(string key, Action<T1, T2, T3> action)
		{
			members.Add(key, action);
		}

		public void Add<T1, T2, T3, T4>(string key, Action<T1, T2, T3, T4> action)
		{
			members.Add(key, action);
		}

		public void Add<T1, T2, T3, T4, T5>(string key, Action<T1, T2, T3, T4, T5> action)
		{
			members.Add(key, action);
		}

		public void Add<T1, T2, T3, T4, T5, T6>(string key, Action<T1, T2, T3, T4, T5, T6> action)
		{
			members.Add(key, action);
		}

		public void Add<T1, T2, T3, T4, T5, T6, T7>(string key, Action<T1, T2, T3, T4, T5, T6, T7> action)
		{
			members.Add(key, action);
		}
	}
}
