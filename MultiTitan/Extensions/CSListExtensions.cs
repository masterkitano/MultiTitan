using MultiTitan.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MultiTitan.Extensions
{
	public static class MTListExtensions
	{
		public static MTList<T> ToMTList<T>(this IEnumerable<T> source)
		{
			MTList<T> list = new MTList<T>();
			list.AddRange(source);
			return list;
		}
	}
}
