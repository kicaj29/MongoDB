using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiUsageExamples.Tests
{
    internal class NUnitTextWriter : TextWriter
    {
        private readonly TextWriter _originalWriter;

        public NUnitTextWriter(TextWriter originalWriter)
        {
            _originalWriter = originalWriter;
        }

        public override Encoding Encoding => _originalWriter.Encoding;

        public override void Write(char value)
        {
            TestContext.Progress.Write(value);
            _originalWriter.Write(value);
        }

        public override void Write(string value)
        {
            TestContext.Progress.Write(value);
            _originalWriter.Write(value);
        }

        public override void WriteLine(string value)
        {
            TestContext.Progress.WriteLine(value);
            _originalWriter.WriteLine(value);
        }
    }
}
