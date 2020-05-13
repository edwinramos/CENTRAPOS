using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WEBPOS.DataAccess.Repository
{
	interface IRepository<T> where T : IDisposable
	{
		IEnumerable<T> Read(T value);
		IEnumerable<T> ReadAll();
		IQueryable<T> ReadAllQueryable();
		IEnumerable<T> ReadAllQueryable(T value);
	}
}
