using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebForumsApp.Utility
{
    public interface IPostFormater
    {
        string Beautify(string text);
    }
}
