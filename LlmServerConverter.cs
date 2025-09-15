﻿using lcLLM.Servers.Abstractions;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Reflection;


namespace lcLLM.Models
{
    public class LlmServerConverter : TypeConverter
    {
        private static readonly LlmServer[] Servers = [.. Assembly.GetExecutingAssembly().GetTypes()
                .Where(type => type.IsSubclassOf(typeof(LlmServer)) && !type.IsAbstract)
                .Select(type => (LlmServer)Activator.CreateInstance(type))];

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context) => true;

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context) => true;

        public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context) => new(Servers);

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
            => destinationType == typeof(string);

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
            => destinationType == typeof(string)
            ? ((LlmServer)value).Name
            : base.ConvertTo(context, culture, value, destinationType);

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
            => sourceType == typeof(string);

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
            => value is string stringValue
            ? Servers.FirstOrDefault(server => server.Name == stringValue)
            : base.ConvertFrom(context, culture, value);
    }
}
