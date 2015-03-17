using System;
using System.Collections.Generic;
using FluentAssertions;
using WatiN.Core;
using WatiN.Core.Constraints;
using WatiN.Core.Exceptions;

namespace ExecutableSpecifications.Core.Watin
{
    public abstract class WatinWebPage : Page
    {
        private readonly Dictionary<Type, Func<Constraint, Element>> _typeToElement;

        public WatinWebPage()
        {
            _typeToElement = new Dictionary<Type, Func<Constraint, Element>>
                             {
                                 {typeof (TextField),  c => Document.TextField  (c)},
                                 {typeof (Button),     c => Document.Button     (c)},
                                 {typeof (CheckBox),   c => Document.CheckBox   (c)},
                                 {typeof (SelectList), c => Document.SelectList (c)},
                                 {typeof (Link),       c => Document.Link       (c)},
                                 {typeof (Table),      c => Document.Table      (c)},
                                 {typeof (RadioButton),c => Document.RadioButton(c)},
                                 {typeof (ListItem),   c => Document.ListItem   (c)},
                                 {typeof (Para),       c => Document.Para       (c)},
                                 {typeof (Element),    c => Document.Element    (c)},
                                 {typeof (Div),        c => Document.Div        (c)},
                                 {typeof (Image),      c => Document.Image      (c)},
                             };
        }

        protected abstract string UrlFragment { get; }

        protected virtual string PageTitleSegment
        {
            get { return string.Empty; }
        }

        protected virtual string QueryString
        {
            get { return string.Empty; }
        }

        protected internal virtual string PageUrl
        {
            get { return Server + UrlFragment + QueryString; }
        }

        abstract public string Server { get; }

        protected T Get<T>(string id = null, string name = null, string value = null,
            string selector = null, string text = null, string alt = null, Tuple<string, string> attribute = null ) where T : Element
        {
            var elementType = typeof (T);


            if (!_typeToElement.ContainsKey(elementType))
            {
                throw new NotImplementedException(String.Format("Don't know how to find elements of type {0}", elementType));
            }
            Constraint c;
            if (!String.IsNullOrEmpty(id))
            {
                c = Find.ById(id);
            }
            else if (!String.IsNullOrEmpty(name))
            {
                c = Find.ByName(name);
            }
            else if (!String.IsNullOrEmpty(value))
            {
                c = Find.ByValue(value);
            }
            else if (!String.IsNullOrEmpty(selector))
            {
                c = Find.BySelector(selector);
            }
            else if (!String.IsNullOrEmpty(text))
            {
                c = Find.ByText(text);
            }
            else if (!String.IsNullOrEmpty(alt))
            {
                c = Find.ByAlt(alt);
            }
            else if (attribute != null)
            {
                // fixup href (as best we can) if it wasn't given as a full URL
                if ((attribute.Item1.ToLower() == "href") && (!attribute.Item2.ToLower().StartsWith("http")))
                {
                    attribute = Tuple.Create(attribute.Item1, Server + attribute.Item2);
                }
                c = Find.By(attribute.Item1, attribute.Item2);
            }
            else
            {
                return null;
            }
            var result = _typeToElement[elementType](c) as T;
            if (result != null && result.Exists) return result;

            throw new ElementNotFoundException(elementType.ToString(), c.ToString(), Document.Url,result);
        }

        public void Load(IE browser, bool validatePageTitle = false, string url = "")
        {
            if (String.IsNullOrEmpty(url))
            {
                url = PageUrl;
            }
            browser.GoTo(url);
            browser.WaitForComplete();

            if (!validatePageTitle)
            {
                return;
            }
            ValidatePageTitle(browser);
        }

        public void ValidatePageTitle(IE browser)
        {
            if (String.IsNullOrEmpty(PageTitleSegment))
            {
                return;
            }
            browser.Title.Should().Contain(PageTitleSegment, "Page Title should contain {0}", PageTitleSegment);
        }
    }
}