
// load the source 

using Compiler.Core;
using Core.Script;

var path = @"D:\source.txt";
var source = Extension.Scan(path);

var scanner = new Scanner();
scanner.ScanTokens(source);

var generator = new AstTreeGenerator();
generator.DefineAst("D:\\LastDance\\Reverse\\Core\\Core", "Expression", new List<string>()
{
     "Binary   : Expression left, Token @operator, Expression right",
     "Grouping : Expression expression",
     "Literal  : Object value",
     "Unary    : Token @operator, Expression right"
});


var zzz = 1;