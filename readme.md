# Epinova.JsResourceHandler

Simple module to include Episerver resource files/translation files for use in JavaScript.

## Example: 

You have an XML-file `/lang/myNiceFile.NO.xml`. After installing this module, this is accessible via 

    <script src="http://yourhostname/jsl10n/myNiceFile">
    </script>

It will look for `myNiceFile.NO.xml` if you're on the NO-based hostname. If you're on a host using Swedish, it will look for `myNiceFile.SV.xml`. If none of those exist, it'll simply look for `myNiceFile.xml`. If it has exactly one `language` node, that will be the root of the output object. If it has multiple languages in one file, the `languages` element will be the root.

The language files must be contained in `/lang/`, seen from your web project. This is the default location from which Episerver picks up stuff anyways.

The object it outputs is called `jsl10n` (`window.jsl10n`).

Append `?debug=true` to the URL to get indented JSON for debugging/inspection purposes.

### Example usage 1: Normal JS

    $(function() {
    	var cuteTextNode = window.jsl10n.articlepage.myCuteTextNode;
    })

### Example usage 2: Angular language service

    var translator = function () {
        return window.jsl10n;
    };
    yourApp.factory('translator', [translator]);

Then inject it into your controllers as `translator`.