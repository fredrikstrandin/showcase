using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestdataGenerator.Model;

public class TestPersonnummer
{
    public string? next { get; set; }
    public int resultCount { get; set; }
    public int offset { get; set; }
    public int limit { get; set; }
    public int queryTime { get; set; }
    public Result[]? results { get; set; }
}

public class Result
{
    public string? testpersonnummer { get; set; }
}
