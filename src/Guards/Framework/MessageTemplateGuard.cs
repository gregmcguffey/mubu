﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BagOUtils.Guards.Framework
{
    /// <summary>
    /// Guard that uses a template to generate a message that includes 
    /// the parameter/argument/variable (item) name. The template accepts a single 
    /// token for the item name.
    /// </summary>
    public class MessageTemplateGuard<TValue, TException>
        where TException : Exception
    {
        private readonly TValue value;

        private Func<bool> test;
        private Func<string, TException> exceptionBuilder;
        private string template;
        private string nameTemplate;
        private string itemName;

        public MessageTemplateGuard(TValue value)
        {
            this.value = value;
        }

        public MessageTemplateGuard<TValue, TException> TestToExecute(Func<bool> test)
        {
            this.test = test;
            return this;
        }

        public MessageTemplateGuard<TValue, TException> ExceptionBuilder(Func<string, TException> builder)
        {
            this.exceptionBuilder = builder;
            return this;
        }

        public MessageTemplateGuard<TValue, TException> TemplateUsed(string template)
        {
            this.template = template;
            return this;
        }

        public MessageTemplateGuard<TValue, TException> NameTemplateUsed(string nameTemplate)
        {
            this.nameTemplate = nameTemplate;
            return this;
        }

        public MessageTemplateGuard<TValue, TException> ForItem(string itemName)
        {
            this.itemName = itemName;
            return this;
        }

        public TValue Guard()
        {
            // Desired result is a true test.
            if(!this.test())
            {
                var message = BuildMessage(this.itemName, this.template, this.nameTemplate);
                var ex = this.exceptionBuilder(message);
                throw ex;
            }
            return this.value;
        }

        private string BuildMessage(string itemName, string template, string nameTemplate)
        {
            Func<string> formatBuilder = () => string.Format(template, itemName);
            Func<string> nameBuilder = () => nameTemplate.Replace("{item}", itemName);

            Func<string, bool> isSet = t => !string.IsNullOrWhiteSpace(t);

            // If both templates are set, use the string.Format one,
            // which likely is faster.
            var message = isSet(template)
                ? formatBuilder()
                : nameBuilder();

            return message;
        }
    }
}
