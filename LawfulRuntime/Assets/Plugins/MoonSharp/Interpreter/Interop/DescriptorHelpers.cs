using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using MoonSharp.Interpreter.Compatibility;

namespace MoonSharp.Interpreter.Interop
{
	/// <summary>
	/// Helper extension methods used to simplify some parts of userdata descriptor implementations
	/// </summary>
	public static class DescriptorHelpers
	{
		/// <summary>
		/// Determines whether a
		/// <see cref="MoonSharpVisibleAttribute" /> or a <see cref="MoonSharpHiddenAttribute" />  is changing visibility of a member
		/// to scripts.
		/// </summary>
		/// <param name="mi">The member to check.</param>
		/// <returns>
		/// <c>true</c> if visibility is forced visible,
		/// <c>false</c> if visibility is forced hidden or the specified MemberInfo is null,
		/// <c>if no attribute was found</c>
		/// </returns>
		/// <exception cref="System.InvalidOperationException">If both MoonSharpHiddenAttribute and MoonSharpVisibleAttribute are specified and they convey different messages.</exception>
		public static bool? GetVisibilityFromAttributes(this MemberInfo mi)
		{
			if (mi == null)
				return false;

			MoonSharpVisibleAttribute va = mi.GetCustomAttributes(true).OfType<MoonSharpVisibleAttribute>().SingleOrDefault();
			MoonSharpHiddenAttribute ha = mi.GetCustomAttributes(true).OfType<MoonSharpHiddenAttribute>().SingleOrDefault();

			if (va != null && ha != null && va.Visible)
				throw new InvalidOperationException(string.Format("A member ('{0}') can't have discording MoonSharpHiddenAttribute and MoonSharpVisibleAttribute.", mi.Name));
			else if (ha != null)
				return false;
			else if (va != null)
				return va.Visible;
			else
				return null;
		}

		public static bool IsDelegateType(this Type t)
		{
			return Framework.Do.IsAssignableFrom(typeof(Delegate), t);
		}

		/// <summary>
		/// Gets the visibility of the type as a string
		/// </summary>
		public static string GetClrVisibility(this Type type)
		{
#if NETFX_CORE
			var t = type.GetTypeInfo();
#else
			Type t = type;
#endif
			if (t.IsPublic || t.IsNestedPublic)
				return "public";
			if ((t.IsNotPublic && (!t.IsNested)) || (t.IsNestedAssembly))
				return "internal";
			if (t.IsNestedFamORAssem)
				return "protected-internal";
			if (t.IsNestedFamANDAssem || t.IsNestedFamily)
				return "protected";
			if (t.IsNestedPrivate)
				return "private";
			return "unknown";
		}

		/// <summary>
		/// Gets a string representing visibility of the given member type
		/// </summary>
		public static string GetClrVisibility(this FieldInfo info)
		{
			if (info.IsPublic)
				return "public";
			if (info.IsAssembly)
				return "internal";
			if (info.IsFamilyOrAssembly)
				return "protected-internal";
			if (info.IsFamilyAndAssembly || info.IsFamily)
				return "protected";
			if (info.IsPrivate)
				return "private";

			return "unknown";
		}

		/// <summary>
		/// Gets a string representing visibility of the given member type
		/// </summary>
		public static string GetClrVisibility(this PropertyInfo info)
		{
			MethodInfo gm = Framework.Do.GetGetMethod(info);
			MethodInfo sm = Framework.Do.GetSetMethod(info);

			string gv = (gm != null) ? GetClrVisibility(gm) : "private";
			string sv = (sm != null) ? GetClrVisibility(sm) : "private";

			if (gv == "public" || sv == "public")
				return "public";
			else if (gv == "internal" || sv == "internal")
				return "internal";
			else
				return gv;
		}

		/// <summary>
		/// Gets a string representing visibility of the given member type
		/// </summary>
		public static string GetClrVisibility(this MethodBase info)
		{
			if (info.IsPublic)
				return "public";
			if (info.IsAssembly)
				return "internal";
			if (info.IsFamilyOrAssembly)
				return "protected-internal";
			if (info.IsFamilyAndAssembly || info.IsFamily)
				return "protected";
			if (info.IsPrivate)
				return "private";

			return "unknown";
		}




		/// <summary>
		/// Determines whether the specified PropertyInfo is visible publicly (either the getter or the setter is public).
		/// </summary>
		/// <param name="pi">The PropertyInfo.</param>
		/// <returns></returns>
		public static bool IsPropertyInfoPublic(this PropertyInfo pi)
		{
			MethodInfo getter = Framework.Do.GetGetMethod(pi);
			MethodInfo setter = Framework.Do.GetSetMethod(pi);

			return (getter != null && getter.IsPublic) || (setter != null && setter.IsPublic);
		}

		/// <summary>
		/// Gets the list of metamethod names from attributes - in practice the list of metamethods declared through
		/// <see cref="MoonSharpUserDataMetamethodAttribute" /> .
		/// </summary>
		/// <param name="mi">The mi.</param>
		/// <returns></returns>
		public static List<string> GetMetaNamesFromAttributes(this MethodInfo mi)
		{
			return mi.GetCustomAttributes(typeof(MoonSharpUserDataMetamethodAttribute), true)
				.OfType<MoonSharpUserDataMetamethodAttribute>()
				.Select(a => a.Name)
				.ToList();
		}

		/// <summary>
		/// Gets the Types implemented in the assembly, catching the ReflectionTypeLoadException just in case..
		/// </summary>
		/// <param name="asm">The assebly</param>
		/// <returns></returns>
		public static Type[] SafeGetTypes(this Assembly asm)
		{
			try
			{
				return Framework.Do.GetAssemblyTypes(asm);
			}
			catch (ReflectionTypeLoadException)
			{
				return new Type[0];
			}
		}




		/// <summary>
		/// Gets the name of a conversion method to be exposed to Lua scripts
		/// </summary>
		/// <param name="type">The type.</param>
		/// <returns></returns>
		public static string GetConversionMethodName(this Type type)
		{
			StringBuilder sb = new StringBuilder(type.Name);

			for (int i = 0; i < sb.Length; i++)
				if (!char.IsLetterOrDigit(sb[i])) sb[i] = '_';

			return "__to" + sb.ToString();
		}


		/// <summary>
		/// Gets all implemented types by a given type
		/// </summary>
		/// <param name="t">The t.</param>
		/// <returns></returns>
		public static IEnumerable<Type> GetAllImplementedTypes(this Type t)
		{
			for (Type ot = t; ot != null; ot = Framework.Do.GetBaseType(ot))
				yield return ot;

			foreach (Type it in Framework.Do.GetInterfaces(t))
				yield return it;
		}
	}
}
