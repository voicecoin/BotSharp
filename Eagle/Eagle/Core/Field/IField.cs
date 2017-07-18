using System;
using System.Collections.Generic;
using System.Text;
using Eagle.Models;

namespace Eagle.Core
{
    public interface IFieldModel
    {
        IEnumerable<string> FieldConfigNames();
    }
}
