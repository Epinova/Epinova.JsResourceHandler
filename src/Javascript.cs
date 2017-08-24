using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Epinova.JsResourceHandler
{
    /// <summary>
    /// Adding some simple js
    /// Compression; https://jscompress.com/
    /// Then replaced " with '
    /// </summary>
    internal static class Javascript
    {
        /// <summary>
        /// Reduce Polyfill; needed for IE6-8
        /// Source: https://developer.mozilla.org/en-US/docs/Web/JavaScript/Reference/Global_Objects/Array/Reduce#Polyfill
        /// </summary>
        public static string PolyfillReduce = "Array.prototype.reduce||Object.defineProperty(Array.prototype,'reduce',{value:function(e){if(null===this)throw new TypeError('Array.prototype.reduce called on null or undefined');if('function'!=typeof e)throw new TypeError(e+' is not a function');var r,t=Object(this),o=t.length>>>0,n=0;if(arguments.length>=2)r=arguments[1];else{for(;n<o&&!(n in t);)n++;if(n>=o)throw new TypeError('Reduce of empty array with no initial value');r=t[n++]}for(;n<o;)n in t&&(r=e(r,t[n],n,t)),n++;return r}});";

        /// <summary>
        /// Translate function
        /// Usage: window.jsl10n.translate("articlePage/myNice/text")
        /// Returns undefined if no translation exists
        /// Compressed: TranslateFunction.js
        /// </summary>
        public static string TranslateFunction = "window.jsl10n.translate=function(n){return n.split('/').reduce(function(n,t){return void 0===n||null===n?n:n[t]},window.jsl10n)};";
    }
}
