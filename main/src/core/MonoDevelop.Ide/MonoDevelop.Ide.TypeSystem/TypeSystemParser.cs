// 
// ITypeSystemParser.cs
//  
// Author:
//       Mike Krüger <mkrueger@novell.com>
// 
// Copyright (c) 2011 Mike Krüger <mkrueger@novell.com>
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using System;
using System.IO;
using ICSharpCode.NRefactory.TypeSystem;
using MonoDevelop.Projects;
using MonoDevelop.Core.Text;
using Microsoft.CodeAnalysis;
using System.Threading.Tasks;
using System.Threading;
using MonoDevelop.Ide.Editor.Projection;

namespace MonoDevelop.Ide.TypeSystem
{
	public sealed class ParseOptions
	{
		public string FileName { get; set; } 

		public ITextSource Content { get; set; }

		public MonoDevelop.Projects.Project Project { get; set; }

		public Document RoslynDocument { get; set; }
	}

	public class ParsedDocumentProjection 
	{
		public ParsedDocument ParsedDocument { get; private set; }
		public Projection Projection { get; private set;}

		public ParsedDocumentProjection (ParsedDocument parsedDocument, Projection projection)
		{
			this.ParsedDocument = parsedDocument;
			this.Projection = projection;
		}
	}

	/// <summary>
	/// A type system parser provides a ParsedDocument (which just adds some more information to a IUnresolvedFile) for
	/// a given file. This is required for adding information to the type system service to make the file contents available
	/// for type lookup (code completion, resolving etc.).
	/// </summary>
	public abstract class TypeSystemParser 
	{
		/// <summary>
		/// Parse the specified file. The file content is provided as text reader.
		/// </summary>
		/// <param name='storeAst'>
		/// If set to <c>true</c> the ast should be stored in the parsed document.
		/// </param>
		/// <param name='fileName'>
		/// The name of the file.
		/// </param>
		/// <param name='content'>
		/// A text reader providing the file contents.
		/// </param>
		/// <param name='project'>
		/// The project the file belongs to.
		/// </param>
		public abstract Task<ParsedDocument> Parse (ParseOptions options, CancellationToken cancellationToken = default(CancellationToken));

//		/// <summary>
//		/// Parse the specified file. The file content should be read by the type system parser.
//		/// </summary>
//		/// <param name='storeAst'>
//		/// If set to <c>true</c> the ast should be stored in the parsed document.
//		/// </param>
//		/// <param name='fileName'>
//		/// The name of the file.
//		/// </param>
//		/// <param name='project'>
//		/// The project the file belongs to.
//		/// </param>
//		public virtual Task<ParsedDocument> Parse (bool storeAst, string fileName, MonoDevelop.Projects.Project project = null, CancellationToken cancellationToken = default(CancellationToken))
//		{
//			var src = StringTextSource.ReadFrom (fileName);
//			return Parse (storeAst, fileName, src, project, cancellationToken);
//		}
//
		public virtual bool CanGenerateProjection (string mimeType, string buildAction, string[] supportedLanguages)
		{
			return false;
		}

		public virtual Task<Projection> GenerateProjection (ParseOptions options, CancellationToken cancellationToken = default(CancellationToken))
		{
			throw new NotSupportedException ();
		}

		public virtual Task<ParsedDocumentProjection> GenerateParsedDocumentProjection (ParseOptions options, CancellationToken cancellationToken = default(CancellationToken))
		{
			throw new NotSupportedException ();
		}

	}
}

