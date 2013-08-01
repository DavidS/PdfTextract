using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using PdfSharp.Pdf;
using PdfSharp.Pdf.Content;
using PdfSharp.Pdf.Content.Objects;
using PdfSharp.Pdf.IO;

namespace PdfTextract.Tester
{
    class Program
    {
        public static IEnumerable<string> GetPdfFiles()
        {
            return Directory.GetFiles(Path.Combine("..", "..", "..", "PdfTextract.Tests", "Data", "PDFs"), "*.pdf", SearchOption.AllDirectories);
        }

        static void Main(string[] args)
        {
            foreach (var pdfFileName in GetPdfFiles())
            {
                Console.Out.WriteLine(pdfFileName);
                using (var pdfFile = PdfReader.Open(pdfFileName, PdfDocumentOpenMode.ReadOnly))
                {
                    foreach (var page in pdfFile.Pages.OfType<PdfPage>())
                    {
                        Write(ContentReader.ReadContent(page));
                    }
                }
            }
        }

        private static void Write(CArray obj)
        {
            Console.Write("[ ");
            foreach (var element in obj)
            {
                Write(element);
                Console.Write(", ");
            }
            Console.Write(" ]");
        }

        private static void Write(CComment obj)
        {
            Console.Write("/* {0} */", obj.Text);
        }

        private static void Write(CInteger obj)
        {
            Console.Write("int:{0}", obj.Value);
        }

        private static void Write(CName obj)
        {
            Console.Write("name:{0}", obj.Name);
        }

        private static void Write(CNumber obj)
        {
            Console.Write("num:{0}", obj.ToString());
        }

        private static void Write(CObject obj)
        {
            if (obj is CArray)
                Write((CArray)obj);
            else if (obj is CComment)
                Write((CComment)obj);
            else if (obj is CInteger)
                Write((CInteger)obj);
            else if (obj is CName)
                Write((CName)obj);
            else if (obj is CNumber)
                Write((CNumber)obj);
            else if (obj is COperator)
                Write((COperator)obj);
            else if (obj is CReal)
                Write((CReal)obj);
            else if (obj is CSequence)
                Write((CSequence)obj);
            else if (obj is CString)
                Write((CString)obj);
            else
                throw new NotImplementedException(obj.GetType().AssemblyQualifiedName);
        }

        private static void Write(COperator obj)
        {
            Console.Write("op:{0}(", obj.Name);
            foreach (var op in obj.Operands)
            {
                Write(op);
                Console.Write(", ");
            }
            Console.Write(")");
        }

        private static void Write(CReal obj)
        {
            Console.Write("real:{0}", obj.Value);
        }

        private static void Write(CSequence obj)
        {
            foreach (var element in obj)
            {
                Write(element);
                Console.WriteLine();
            }
        }

        private static void Write(CString obj)
        {
            Console.Write("str:{0}", obj.Value);
        }
    }
}
