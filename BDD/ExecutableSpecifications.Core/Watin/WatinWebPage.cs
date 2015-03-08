using System;
using System.Collections.Generic;
using FluentAssertions;
using WatiN.Core;
using WatiN.Core.Constraints;

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

        protected T Get<T>(string id = "", string name = "", string value = "", string selector = "", string text = "") where T : Element
        {
            var elementType = typeof (T);

            if (!_typeToElement.ContainsKey(elementType))
            {
                return null;
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
            else
            {
                return null;
            }
            return _typeToElement[elementType](c) as T;
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