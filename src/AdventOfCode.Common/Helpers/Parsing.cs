using System;

namespace AdventOfCode.Common;

public static class Parsing {

    public static int ParseInt(ReadOnlySpan<char> span) {
        int result = 0;
        foreach (char c in span)
            result = result * 10 + c - '0';
        return result;
    }

}
