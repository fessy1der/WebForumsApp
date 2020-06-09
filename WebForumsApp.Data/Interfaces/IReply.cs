using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WebForumsApp.Data.Interfaces
{
    public interface IReply
    {
        Reply GetById(int id);
        Task Edit(int id, string message);
        Task Delete(int id);
    }
}
