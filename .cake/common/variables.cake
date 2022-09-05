///////////////////////////////////////////////////////////////////////////////
// ARGUMENTS
///////////////////////////////////////////////////////////////////////////////

using System;
using System.Globalization;

#break

var _base_ = MakeAbsolute(new DirectoryPath(GetRootDirectory()));
var _root_ = MakeAbsolute(new DirectoryPath($"{_base_}"));
var _nlog_ = MakeAbsolute(new FilePath($"{_root_}/.logs/nlog.log"));