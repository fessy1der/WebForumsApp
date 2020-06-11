using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebForumsApp.Utility
{
    public class PostFormater : IPostFormater
    {
        public string Beautify(string text)
        {
            var postWithSpaces = TransformSpaces(text);
            var postCodeFormated = TransformCodeTags(postWithSpaces);
            return postCodeFormated;
        }

        private static string TransformSpaces(string post)
        {
            return post.Replace(Environment.NewLine, "<br />");
        }

        private static string TransformCodeTags(string post)
        {
            var head = post.Replace("[code]", "<pre>");
            return head.Replace(@"[/code]", "</pre>");
        }
    }
}
