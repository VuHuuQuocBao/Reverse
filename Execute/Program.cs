
// load the source 

using Compiler.Core;

var path = @"D:\source.txt";
var source = Extension.Scan(path);

var scanner = new Scanner();
scanner.ScanTokens(source);

var zzz = 1;