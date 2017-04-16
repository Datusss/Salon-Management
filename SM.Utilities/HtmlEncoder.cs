using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace SM.Utilities
{
    public class HtmlEncoder
    {
        public static string DecodeValue(string value)
        {
            StringBuilder builder = new StringBuilder();
            StringReader reader = new StringReader(value);
            int num = reader.Read();
            while (num != -1)
            {
                StringBuilder builder2 = new StringBuilder();
                while ((num != 0x26) && (num != -1))
                {
                    builder2.Append((char)num);
                    num = reader.Read();
                }
                builder.Append(builder2.ToString());
                if (num == 0x26)
                {
                    builder2 = new StringBuilder();
                    while ((num != 0x3b) && (num != -1))
                    {
                        builder2.Append((char)num);
                        num = reader.Read();
                    }
                    if (num == 0x3b)
                    {
                        num = reader.Read();
                        builder2.Append(';');
                        if (builder2[1] == '#')
                        {
                            int result = -1;
                            if (int.TryParse(builder2.ToString().Substring(2, builder2.Length - 3), out result))
                            {
                                builder.Append((char)result);
                            }
                            else if (int.TryParse(builder2.ToString().Substring(3, builder2.Length - 4), NumberStyles.HexNumber, null, out result))
                            {
                                builder.Append((char)result);
                            }
                        }
                        else
                        {
                            switch (builder2.ToString())
                            {
                                case "&lt;":
                                    {
                                        builder.Append("<");
                                        continue;
                                    }
                                case "&gt;":
                                    {
                                        builder.Append(">");
                                        continue;
                                    }
                                case "&quot;":
                                    {
                                        builder.Append("\"");
                                        continue;
                                    }
                                case "&amp;":
                                    {
                                        builder.Append("&");
                                        continue;
                                    }
                                case "&Aacute;":
                                    {
                                        builder.Append('\x00c1');
                                        continue;
                                    }
                                case "&aacute;":
                                    {
                                        builder.Append('\x00e1');
                                        continue;
                                    }
                                case "&Acirc;":
                                    {
                                        builder.Append('\x00c2');
                                        continue;
                                    }
                                case "&acirc;":
                                    {
                                        builder.Append('\x00e2');
                                        continue;
                                    }
                                case "&acute;":
                                    {
                                        builder.Append('\x00b4');
                                        continue;
                                    }
                                case "&AElig;":
                                    {
                                        builder.Append('\x00c6');
                                        continue;
                                    }
                                case "&aelig;":
                                    {
                                        builder.Append('\x00e6');
                                        continue;
                                    }
                                case "&Agrave;":
                                    {
                                        builder.Append('\x00c0');
                                        continue;
                                    }
                                case "&agrave;":
                                    {
                                        builder.Append('\x00e0');
                                        continue;
                                    }
                                case "&alefsym;":
                                    {
                                        builder.Append('ℵ');
                                        continue;
                                    }
                                case "&Alpha;":
                                    {
                                        builder.Append('Α');
                                        continue;
                                    }
                                case "&alpha;":
                                    {
                                        builder.Append('α');
                                        continue;
                                    }
                                case "&and;":
                                    {
                                        builder.Append('∧');
                                        continue;
                                    }
                                case "&ang;":
                                    {
                                        builder.Append('∠');
                                        continue;
                                    }
                                case "&Aring;":
                                    {
                                        builder.Append('\x00c5');
                                        continue;
                                    }
                                case "&aring;":
                                    {
                                        builder.Append('\x00e5');
                                        continue;
                                    }
                                case "&asymp;":
                                    {
                                        builder.Append('≈');
                                        continue;
                                    }
                                case "&Atilde;":
                                    {
                                        builder.Append('\x00c3');
                                        continue;
                                    }
                                case "&atilde;":
                                    {
                                        builder.Append('\x00e3');
                                        continue;
                                    }
                                case "&Auml;":
                                    {
                                        builder.Append('\x00c4');
                                        continue;
                                    }
                                case "&auml;":
                                    {
                                        builder.Append('\x00e4');
                                        continue;
                                    }
                                case "&bdquo;":
                                    {
                                        builder.Append('„');
                                        continue;
                                    }
                                case "&Beta;":
                                    {
                                        builder.Append('Β');
                                        continue;
                                    }
                                case "&beta;":
                                    {
                                        builder.Append('β');
                                        continue;
                                    }
                                case "&brvbar;":
                                    {
                                        builder.Append('\x00a6');
                                        continue;
                                    }
                                case "&bull;":
                                    {
                                        builder.Append('•');
                                        continue;
                                    }
                                case "&cap;":
                                    {
                                        builder.Append('∩');
                                        continue;
                                    }
                                case "&Ccedil;":
                                    {
                                        builder.Append('\x00c7');
                                        continue;
                                    }
                                case "&ccedil;":
                                    {
                                        builder.Append('\x00e7');
                                        continue;
                                    }
                                case "&cedil;":
                                    {
                                        builder.Append('\x00b8');
                                        continue;
                                    }
                                case "&cent;":
                                    {
                                        builder.Append('\x00a2');
                                        continue;
                                    }
                                case "&Chi;":
                                    {
                                        builder.Append('Χ');
                                        continue;
                                    }
                                case "&chi;":
                                    {
                                        builder.Append('χ');
                                        continue;
                                    }
                                case "&circ;":
                                    {
                                        builder.Append('ˆ');
                                        continue;
                                    }
                                case "&clubs;":
                                    {
                                        builder.Append('♣');
                                        continue;
                                    }
                                case "&cong;":
                                    {
                                        builder.Append('≅');
                                        continue;
                                    }
                                case "&copy;":
                                    {
                                        builder.Append('\x00a9');
                                        continue;
                                    }
                                case "&crarr;":
                                    {
                                        builder.Append('↵');
                                        continue;
                                    }
                                case "&cup;":
                                    {
                                        builder.Append('∪');
                                        continue;
                                    }
                                case "&curren;":
                                    {
                                        builder.Append('\x00a4');
                                        continue;
                                    }
                                case "&dagger;":
                                    {
                                        builder.Append('†');
                                        continue;
                                    }
                                case "&Dagger;":
                                    {
                                        builder.Append('‡');
                                        continue;
                                    }
                                case "&darr;":
                                    {
                                        builder.Append('↓');
                                        continue;
                                    }
                                case "&dArr;":
                                    {
                                        builder.Append('⇓');
                                        continue;
                                    }
                                case "&deg;":
                                    {
                                        builder.Append('\x00b0');
                                        continue;
                                    }
                                case "&Delta;":
                                    {
                                        builder.Append('Δ');
                                        continue;
                                    }
                                case "&delta;":
                                    {
                                        builder.Append('δ');
                                        continue;
                                    }
                                case "&diams;":
                                    {
                                        builder.Append('♦');
                                        continue;
                                    }
                                case "&divide;":
                                    {
                                        builder.Append('\x00f7');
                                        continue;
                                    }
                                case "&Eacute;":
                                    {
                                        builder.Append('\x00c9');
                                        continue;
                                    }
                                case "&eacute;":
                                    {
                                        builder.Append('\x00e9');
                                        continue;
                                    }
                                case "&Ecirc;":
                                    {
                                        builder.Append('\x00ca');
                                        continue;
                                    }
                                case "&ecirc;":
                                    {
                                        builder.Append('\x00ea');
                                        continue;
                                    }
                                case "&Egrave;":
                                    {
                                        builder.Append('\x00c8');
                                        continue;
                                    }
                                case "&egrave;":
                                    {
                                        builder.Append('\x00e8');
                                        continue;
                                    }
                                case "&empty;":
                                    {
                                        builder.Append('∅');
                                        continue;
                                    }
                                case "&emsp;":
                                    {
                                        builder.Append(' ');
                                        continue;
                                    }
                                case "&Epsilon;":
                                    {
                                        builder.Append('Ε');
                                        continue;
                                    }
                                case "&epsilon;":
                                    {
                                        builder.Append('ε');
                                        continue;
                                    }
                                case "&equiv;":
                                    {
                                        builder.Append('≡');
                                        continue;
                                    }
                                case "&Eta;":
                                    {
                                        builder.Append('Η');
                                        continue;
                                    }
                                case "&eta;":
                                    {
                                        builder.Append('η');
                                        continue;
                                    }
                                case "&ETH;":
                                    {
                                        builder.Append('\x00d0');
                                        continue;
                                    }
                                case "&eth;":
                                    {
                                        builder.Append('\x00f0');
                                        continue;
                                    }
                                case "&Euml;":
                                    {
                                        builder.Append('\x00cb');
                                        continue;
                                    }
                                case "&euml;":
                                    {
                                        builder.Append('\x00eb');
                                        continue;
                                    }
                                case "&euro;":
                                    {
                                        builder.Append('\x0080');
                                        continue;
                                    }
                                case "&exist;":
                                    {
                                        builder.Append('∃');
                                        continue;
                                    }
                                case "&fnof;":
                                    {
                                        builder.Append('ƒ');
                                        continue;
                                    }
                                case "&forall;":
                                    {
                                        builder.Append('∀');
                                        continue;
                                    }
                                case "&frac12;":
                                    {
                                        builder.Append('\x00bd');
                                        continue;
                                    }
                                case "&frac14;":
                                    {
                                        builder.Append('\x00bc');
                                        continue;
                                    }
                                case "&frac34;":
                                    {
                                        builder.Append('\x00be');
                                        continue;
                                    }
                                case "&fras1;":
                                    {
                                        builder.Append('⁄');
                                        continue;
                                    }
                                case "&Gamma;":
                                    {
                                        builder.Append('Γ');
                                        continue;
                                    }
                                case "&gamma;":
                                    {
                                        builder.Append('γ');
                                        continue;
                                    }
                                case "&ge;":
                                    {
                                        builder.Append('≥');
                                        continue;
                                    }
                                case "&harr;":
                                    {
                                        builder.Append('↔');
                                        continue;
                                    }
                                case "&hArr;":
                                    {
                                        builder.Append('⇔');
                                        continue;
                                    }
                                case "&hearts;":
                                    {
                                        builder.Append('♥');
                                        continue;
                                    }
                                case "&hellip;":
                                    {
                                        builder.Append('…');
                                        continue;
                                    }
                                case "&Iacute;":
                                    {
                                        builder.Append('\x00cd');
                                        continue;
                                    }
                                case "&iacute;":
                                    {
                                        builder.Append('\x00ed');
                                        continue;
                                    }
                                case "&Icirc;":
                                    {
                                        builder.Append('\x00ce');
                                        continue;
                                    }
                                case "&icirc;":
                                    {
                                        builder.Append('\x00ee');
                                        continue;
                                    }
                                case "&iexcl;":
                                    {
                                        builder.Append('\x00a1');
                                        continue;
                                    }
                                case "&Igrave;":
                                    {
                                        builder.Append('\x00cc');
                                        continue;
                                    }
                                case "&igrave;":
                                    {
                                        builder.Append('\x00ec');
                                        continue;
                                    }
                                case "&image;":
                                    {
                                        builder.Append('ℑ');
                                        continue;
                                    }
                                case "&infin;":
                                    {
                                        builder.Append('∞');
                                        continue;
                                    }
                                case "&int;":
                                    {
                                        builder.Append('∫');
                                        continue;
                                    }
                                case "&Iota;":
                                    {
                                        builder.Append('Ι');
                                        continue;
                                    }
                                case "&iota;":
                                    {
                                        builder.Append('ι');
                                        continue;
                                    }
                                case "&iquest;":
                                    {
                                        builder.Append('\x00bf');
                                        continue;
                                    }
                                case "&isin;":
                                    {
                                        builder.Append('∈');
                                        continue;
                                    }
                                case "&Iuml;":
                                    {
                                        builder.Append('\x00cf');
                                        continue;
                                    }
                                case "&iuml;":
                                    {
                                        builder.Append('\x00ef');
                                        continue;
                                    }
                                case "&Kappa;":
                                    {
                                        builder.Append('Κ');
                                        continue;
                                    }
                                case "&kappa;":
                                    {
                                        builder.Append('κ');
                                        continue;
                                    }
                                case "&Lambda;":
                                    {
                                        builder.Append('Λ');
                                        continue;
                                    }
                                case "&lambda;":
                                    {
                                        builder.Append('λ');
                                        continue;
                                    }
                                case "&lang;":
                                    {
                                        builder.Append('〈');
                                        continue;
                                    }
                                case "&laquo;":
                                    {
                                        builder.Append('\x00ab');
                                        continue;
                                    }
                                case "&larr;":
                                    {
                                        builder.Append('←');
                                        continue;
                                    }
                                case "&lArr;":
                                    {
                                        builder.Append('⇐');
                                        continue;
                                    }
                                case "&lceil;":
                                    {
                                        builder.Append('⌈');
                                        continue;
                                    }
                                case "&ldquo;":
                                    {
                                        builder.Append('“');
                                        continue;
                                    }
                                case "&le;":
                                    {
                                        builder.Append('≤');
                                        continue;
                                    }
                                case "&lfloor;":
                                    {
                                        builder.Append('⌊');
                                        continue;
                                    }
                                case "&lowast;":
                                    {
                                        builder.Append('∗');
                                        continue;
                                    }
                                case "&loz;":
                                    {
                                        builder.Append('◊');
                                        continue;
                                    }
                                case "&lrm;":
                                    {
                                        builder.Append('‎');
                                        continue;
                                    }
                                case "&lsaquo;":
                                    {
                                        builder.Append('‹');
                                        continue;
                                    }
                                case "&lsquo;":
                                    {
                                        builder.Append('‘');
                                        continue;
                                    }
                                case "&macr;":
                                    {
                                        builder.Append('\x00af');
                                        continue;
                                    }
                                case "&mdash;":
                                    {
                                        builder.Append('—');
                                        continue;
                                    }
                                case "&micro;":
                                    {
                                        builder.Append('\x00b5');
                                        continue;
                                    }
                                case "&middot;":
                                    {
                                        builder.Append('\x00b7');
                                        continue;
                                    }
                                case "&minus;":
                                    {
                                        builder.Append('−');
                                        continue;
                                    }
                                case "&Mu;":
                                    {
                                        builder.Append('Μ');
                                        continue;
                                    }
                                case "&mu;":
                                    {
                                        builder.Append('μ');
                                        continue;
                                    }
                                case "&nabla;":
                                    {
                                        builder.Append('∇');
                                        continue;
                                    }
                                case "&nbsp;":
                                    {
                                        builder.Append(' ');
                                        continue;
                                    }
                                case "&ndash;":
                                    {
                                        builder.Append('–');
                                        continue;
                                    }
                                case "&ne;":
                                    {
                                        builder.Append('≠');
                                        continue;
                                    }
                                case "&ni;":
                                    {
                                        builder.Append('∋');
                                        continue;
                                    }
                                case "&not;":
                                    {
                                        builder.Append('\x00ac');
                                        continue;
                                    }
                                case "&notin;":
                                    {
                                        builder.Append('∉');
                                        continue;
                                    }
                                case "&nsub;":
                                    {
                                        builder.Append('⊄');
                                        continue;
                                    }
                                case "&Ntilde;":
                                    {
                                        builder.Append('\x00d1');
                                        continue;
                                    }
                                case "&ntilde;":
                                    {
                                        builder.Append('\x00f1');
                                        continue;
                                    }
                                case "&Nu;":
                                    {
                                        builder.Append('Ν');
                                        continue;
                                    }
                                case "&nu;":
                                    {
                                        builder.Append('ν');
                                        continue;
                                    }
                                case "&Oacute;":
                                    {
                                        builder.Append('\x00d3');
                                        continue;
                                    }
                                case "&oacute;":
                                    {
                                        builder.Append('\x00f3');
                                        continue;
                                    }
                                case "&Ocirc;":
                                    {
                                        builder.Append('\x00d4');
                                        continue;
                                    }
                                case "&ocirc;":
                                    {
                                        builder.Append('\x00f4');
                                        continue;
                                    }
                                case "&OElig;":
                                    {
                                        builder.Append('Œ');
                                        continue;
                                    }
                                case "&oelig;":
                                    {
                                        builder.Append('œ');
                                        continue;
                                    }
                                case "&Ograve;":
                                    {
                                        builder.Append('\x00d2');
                                        continue;
                                    }
                                case "&ograve;":
                                    {
                                        builder.Append('\x00f2');
                                        continue;
                                    }
                                case "&oline;":
                                    {
                                        builder.Append('‾');
                                        continue;
                                    }
                                case "&Omega;":
                                    {
                                        builder.Append('Ω');
                                        continue;
                                    }
                                case "&omega;":
                                    {
                                        builder.Append('ω');
                                        continue;
                                    }
                                case "&Omicron;":
                                    {
                                        builder.Append('Ο');
                                        continue;
                                    }
                                case "&omicron;":
                                    {
                                        builder.Append('ο');
                                        continue;
                                    }
                                case "&oplus;":
                                    {
                                        builder.Append('⊕');
                                        continue;
                                    }
                                case "&or;":
                                    {
                                        builder.Append('∨');
                                        continue;
                                    }
                                case "&ordf;":
                                    {
                                        builder.Append('\x00aa');
                                        continue;
                                    }
                                case "&ordm;":
                                    {
                                        builder.Append('\x00ba');
                                        continue;
                                    }
                                case "&Oslash;":
                                    {
                                        builder.Append('\x00d8');
                                        continue;
                                    }
                                case "&oslash;":
                                    {
                                        builder.Append('\x00f8');
                                        continue;
                                    }
                                case "&Otilde;":
                                    {
                                        builder.Append('\x00d5');
                                        continue;
                                    }
                                case "&otilde;":
                                    {
                                        builder.Append('\x00f5');
                                        continue;
                                    }
                                case "&otimes;":
                                    {
                                        builder.Append('⊗');
                                        continue;
                                    }
                                case "&Ouml;":
                                    {
                                        builder.Append('\x00d6');
                                        continue;
                                    }
                                case "&ouml;":
                                    {
                                        builder.Append('\x00f6');
                                        continue;
                                    }
                                case "&para;":
                                    {
                                        builder.Append('\x00b6');
                                        continue;
                                    }
                                case "&part;":
                                    {
                                        builder.Append('∂');
                                        continue;
                                    }
                                case "&permil;":
                                    {
                                        builder.Append('‰');
                                        continue;
                                    }
                                case "&perp;":
                                    {
                                        builder.Append('⊥');
                                        continue;
                                    }
                                case "&Phi;":
                                    {
                                        builder.Append('Φ');
                                        continue;
                                    }
                                case "&phi;":
                                    {
                                        builder.Append('φ');
                                        continue;
                                    }
                                case "&Pi;":
                                    {
                                        builder.Append('Π');
                                        continue;
                                    }
                                case "&pi;":
                                    {
                                        builder.Append('π');
                                        continue;
                                    }
                                case "&piv;":
                                    {
                                        builder.Append('ϖ');
                                        continue;
                                    }
                                case "&plusmn;":
                                    {
                                        builder.Append('\x00b1');
                                        continue;
                                    }
                                case "&pound;":
                                    {
                                        builder.Append('\x00a3');
                                        continue;
                                    }
                                case "&prime;":
                                    {
                                        builder.Append('′');
                                        continue;
                                    }
                                case "&Prime;":
                                    {
                                        builder.Append('″');
                                        continue;
                                    }
                                case "&prod;":
                                    {
                                        builder.Append('∏');
                                        continue;
                                    }
                                case "&prop;":
                                    {
                                        builder.Append('∝');
                                        continue;
                                    }
                                case "&Psi;":
                                    {
                                        builder.Append('Ψ');
                                        continue;
                                    }
                                case "&psi;":
                                    {
                                        builder.Append('ψ');
                                        continue;
                                    }
                                case "&radic;":
                                    {
                                        builder.Append('√');
                                        continue;
                                    }
                                case "&rang;":
                                    {
                                        builder.Append('〉');
                                        continue;
                                    }
                                case "&raquo;":
                                    {
                                        builder.Append('\x00bb');
                                        continue;
                                    }
                                case "&rarr;":
                                    {
                                        builder.Append('→');
                                        continue;
                                    }
                                case "&rArr;":
                                    {
                                        builder.Append('⇒');
                                        continue;
                                    }
                                case "&rceil;":
                                    {
                                        builder.Append('⌉');
                                        continue;
                                    }
                                case "&rdquo;":
                                    {
                                        builder.Append('”');
                                        continue;
                                    }
                                case "&real;":
                                    {
                                        builder.Append('ℜ');
                                        continue;
                                    }
                                case "&reg;":
                                    {
                                        builder.Append('\x00ae');
                                        continue;
                                    }
                                case "&rfloor;":
                                    {
                                        builder.Append('⌋');
                                        continue;
                                    }
                                case "&Rho;":
                                    {
                                        builder.Append('Ρ');
                                        continue;
                                    }
                                case "&rho;":
                                    {
                                        builder.Append('ρ');
                                        continue;
                                    }
                                case "&rlm;":
                                    {
                                        builder.Append('‏');
                                        continue;
                                    }
                                case "&rsaquo;":
                                    {
                                        builder.Append('›');
                                        continue;
                                    }
                                case "&rsquo;":
                                    {
                                        builder.Append('’');
                                        continue;
                                    }
                                case "&sbquo;":
                                    {
                                        builder.Append('‚');
                                        continue;
                                    }
                                case "&Scaron;":
                                    {
                                        builder.Append('Š');
                                        continue;
                                    }
                                case "&scaron;":
                                    {
                                        builder.Append('š');
                                        continue;
                                    }
                                case "&sdot;":
                                    {
                                        builder.Append('⋅');
                                        continue;
                                    }
                                case "&sect;":
                                    {
                                        builder.Append('\x00a7');
                                        continue;
                                    }
                                case "&shy;":
                                    {
                                        builder.Append('\x00ad');
                                        continue;
                                    }
                                case "&Sigma;":
                                    {
                                        builder.Append('Σ');
                                        continue;
                                    }
                                case "&sigma;":
                                    {
                                        builder.Append('σ');
                                        continue;
                                    }
                                case "&sigmaf;":
                                    {
                                        builder.Append('ς');
                                        continue;
                                    }
                                case "&sim;":
                                    {
                                        builder.Append('∼');
                                        continue;
                                    }
                                case "&spades;":
                                    {
                                        builder.Append('♠');
                                        continue;
                                    }
                                case "&sub;":
                                    {
                                        builder.Append('⊂');
                                        continue;
                                    }
                                case "&sube;":
                                    {
                                        builder.Append('⊆');
                                        continue;
                                    }
                                case "&sum;":
                                    {
                                        builder.Append('∑');
                                        continue;
                                    }
                                case "&sup;":
                                    {
                                        builder.Append('⊃');
                                        continue;
                                    }
                                case "&sup1;":
                                    {
                                        builder.Append('\x00b9');
                                        continue;
                                    }
                                case "&sup2;":
                                    {
                                        builder.Append('\x00b2');
                                        continue;
                                    }
                                case "&sup3;":
                                    {
                                        builder.Append('\x00b3');
                                        continue;
                                    }
                                case "&supe;":
                                    {
                                        builder.Append('⊇');
                                        continue;
                                    }
                                case "&szlig;":
                                    {
                                        builder.Append('\x00df');
                                        continue;
                                    }
                                case "&Tau;":
                                    {
                                        builder.Append('Τ');
                                        continue;
                                    }
                                case "&tau;":
                                    {
                                        builder.Append('τ');
                                        continue;
                                    }
                                case "&there4;":
                                    {
                                        builder.Append('∴');
                                        continue;
                                    }
                                case "&Theta;":
                                    {
                                        builder.Append('Θ');
                                        continue;
                                    }
                                case "&theta;":
                                    {
                                        builder.Append('θ');
                                        continue;
                                    }
                                case "&thetasym;":
                                    {
                                        builder.Append('ϑ');
                                        continue;
                                    }
                                case "&thinsp;":
                                    {
                                        builder.Append(' ');
                                        continue;
                                    }
                                case "&THORN;":
                                    {
                                        builder.Append('\x00de');
                                        continue;
                                    }
                                case "&thorn;":
                                    {
                                        builder.Append('\x00fe');
                                        continue;
                                    }
                                case "&tilde;":
                                    {
                                        builder.Append('˜');
                                        continue;
                                    }
                                case "&times;":
                                    {
                                        builder.Append('\x00d7');
                                        continue;
                                    }
                                case "&trade;":
                                    {
                                        builder.Append('™');
                                        continue;
                                    }
                                case "&Uacute;":
                                    {
                                        builder.Append('\x00da');
                                        continue;
                                    }
                                case "&uacute;":
                                    {
                                        builder.Append('\x00fa');
                                        continue;
                                    }
                                case "&uarr;":
                                    {
                                        builder.Append('↑');
                                        continue;
                                    }
                                case "&uArr;":
                                    {
                                        builder.Append('⇑');
                                        continue;
                                    }
                                case "&Ucirc;":
                                    {
                                        builder.Append('\x00db');
                                        continue;
                                    }
                                case "&ucirc;":
                                    {
                                        builder.Append('\x00fb');
                                        continue;
                                    }
                                case "&Ugrave;":
                                    {
                                        builder.Append('\x00d9');
                                        continue;
                                    }
                                case "&ugrave;":
                                    {
                                        builder.Append('\x00f9');
                                        continue;
                                    }
                                case "&uml;":
                                    {
                                        builder.Append('\x00a8');
                                        continue;
                                    }
                                case "&upsih;":
                                    {
                                        builder.Append('ϒ');
                                        continue;
                                    }
                                case "&Upsilon;":
                                    {
                                        builder.Append('Υ');
                                        continue;
                                    }
                                case "&upsilon;":
                                    {
                                        builder.Append('υ');
                                        continue;
                                    }
                                case "&Uuml;":
                                    {
                                        builder.Append('\x00dc');
                                        continue;
                                    }
                                case "&uuml;":
                                    {
                                        builder.Append('\x00fc');
                                        continue;
                                    }
                                case "&weierp;":
                                    {
                                        builder.Append('℘');
                                        continue;
                                    }
                                case "&Xi;":
                                    {
                                        builder.Append('Ξ');
                                        continue;
                                    }
                                case "&xi;":
                                    {
                                        builder.Append('ξ');
                                        continue;
                                    }
                                case "&Yacute;":
                                    {
                                        builder.Append('\x00dd');
                                        continue;
                                    }
                                case "&yacute;":
                                    {
                                        builder.Append('\x00fd');
                                        continue;
                                    }
                                case "&yen;":
                                    {
                                        builder.Append('\x00a5');
                                        continue;
                                    }
                                case "&Yuml;":
                                    {
                                        builder.Append('Ÿ');
                                        continue;
                                    }
                                case "&yuml;":
                                    {
                                        builder.Append('\x00ff');
                                        continue;
                                    }
                                case "&Zeta;":
                                    {
                                        builder.Append('Ζ');
                                        continue;
                                    }
                                case "&zeta;":
                                    {
                                        builder.Append('ζ');
                                        continue;
                                    }
                                case "&zwj;":
                                    {
                                        builder.Append('‍');
                                        continue;
                                    }
                                case "&zwnj;":
                                    {
                                        builder.Append('‌');
                                        continue;
                                    }
                            }
                            builder.Append(builder2.ToString());
                        }
                    }
                    else
                    {
                        builder.Append(builder2.ToString());
                    }
                }
            }
            return builder.ToString();
        }

        public static string EncodeValue(string value)
        {
            StringBuilder builder = new StringBuilder();
            StringReader reader = new StringReader(value);
            for (int i = reader.Read(); i != -1; i = reader.Read())
            {
                switch (i)
                {
                    case 0x20:
                        {
                            builder.Append("&nbsp;");
                            continue;
                        }
                    case 0x22:
                        {
                            builder.Append("\"");
                            continue;
                        }
                    case 0x26:
                        {
                            builder.Append("&amp;");
                            continue;
                        }
                    case 60:
                        {
                            builder.Append("<");
                            continue;
                        }
                    case 0x3e:
                        {
                            builder.Append(">");
                            continue;
                        }
                    case 0x80:
                        {
                            builder.Append("&euro;");
                            continue;
                        }
                    case 160:
                        {
                            builder.Append("&nbsp;");
                            continue;
                        }
                    case 0xa1:
                        {
                            builder.Append("&iexcl;");
                            continue;
                        }
                    case 0xa2:
                        {
                            builder.Append("&cent;");
                            continue;
                        }
                    case 0xa3:
                        {
                            builder.Append("&pound;");
                            continue;
                        }
                    case 0xa4:
                        {
                            builder.Append("&curren;");
                            continue;
                        }
                    case 0xa5:
                        {
                            builder.Append("&yen;");
                            continue;
                        }
                    case 0xa6:
                        {
                            builder.Append("&brvbar;");
                            continue;
                        }
                    case 0xa7:
                        {
                            builder.Append("&sect;");
                            continue;
                        }
                    case 0xa8:
                        {
                            builder.Append("&uml;");
                            continue;
                        }
                    case 0xa9:
                        {
                            builder.Append("&copy;");
                            continue;
                        }
                    case 170:
                        {
                            builder.Append("&ordf;");
                            continue;
                        }
                    case 0xab:
                        {
                            builder.Append("&laquo;");
                            continue;
                        }
                    case 0xac:
                        {
                            builder.Append("&not;");
                            continue;
                        }
                    case 0xad:
                        {
                            builder.Append("&shy;");
                            continue;
                        }
                    case 0xae:
                        {
                            builder.Append("&reg;");
                            continue;
                        }
                    case 0xaf:
                        {
                            builder.Append("&macr;");
                            continue;
                        }
                    case 0xb0:
                        {
                            builder.Append("&deg;");
                            continue;
                        }
                    case 0xb1:
                        {
                            builder.Append("&plusmn;");
                            continue;
                        }
                    case 0xb2:
                        {
                            builder.Append("&sup2;");
                            continue;
                        }
                    case 0xb3:
                        {
                            builder.Append("&sup3;");
                            continue;
                        }
                    case 180:
                        {
                            builder.Append("&acute;");
                            continue;
                        }
                    case 0xb5:
                        {
                            builder.Append("&micro;");
                            continue;
                        }
                    case 0xb6:
                        {
                            builder.Append("&para;");
                            continue;
                        }
                    case 0xb7:
                        {
                            builder.Append("&middot;");
                            continue;
                        }
                    case 0xb8:
                        {
                            builder.Append("&cedil;");
                            continue;
                        }
                    case 0xb9:
                        {
                            builder.Append("&sup1;");
                            continue;
                        }
                    case 0xba:
                        {
                            builder.Append("&ordm;");
                            continue;
                        }
                    case 0xbb:
                        {
                            builder.Append("&raquo;");
                            continue;
                        }
                    case 0xbc:
                        {
                            builder.Append("&frac14;");
                            continue;
                        }
                    case 0xbd:
                        {
                            builder.Append("&frac12;");
                            continue;
                        }
                    case 190:
                        {
                            builder.Append("&frac34;");
                            continue;
                        }
                    case 0xbf:
                        {
                            builder.Append("&iquest;");
                            continue;
                        }
                    case 0xc0:
                        {
                            builder.Append("&Agrave;");
                            continue;
                        }
                    case 0xc1:
                        {
                            builder.Append("&Aacute;");
                            continue;
                        }
                    case 0xc2:
                        {
                            builder.Append("&Acirc;");
                            continue;
                        }
                    case 0xc3:
                        {
                            builder.Append("&Atilde;");
                            continue;
                        }
                    case 0xc4:
                        {
                            builder.Append("&Auml;");
                            continue;
                        }
                    case 0xc5:
                        {
                            builder.Append("&Aring;");
                            continue;
                        }
                    case 0xc6:
                        {
                            builder.Append("&AElig;");
                            continue;
                        }
                    case 0xc7:
                        {
                            builder.Append("&Ccedil;");
                            continue;
                        }
                    case 200:
                        {
                            builder.Append("&Egrave;");
                            continue;
                        }
                    case 0xc9:
                        {
                            builder.Append("&Eacute;");
                            continue;
                        }
                    case 0xca:
                        {
                            builder.Append("&Ecirc;");
                            continue;
                        }
                    case 0xcb:
                        {
                            builder.Append("&Euml;");
                            continue;
                        }
                    case 0xcc:
                        {
                            builder.Append("&Igrave;");
                            continue;
                        }
                    case 0xcd:
                        {
                            builder.Append("&Iacute;");
                            continue;
                        }
                    case 0xce:
                        {
                            builder.Append("&Icirc;");
                            continue;
                        }
                    case 0xcf:
                        {
                            builder.Append("&Iuml;");
                            continue;
                        }
                    case 0xd0:
                        {
                            builder.Append("&ETH;");
                            continue;
                        }
                    case 0xd1:
                        {
                            builder.Append("&Ntilde;");
                            continue;
                        }
                    case 210:
                        {
                            builder.Append("&Ograve;");
                            continue;
                        }
                    case 0xd3:
                        {
                            builder.Append("&Oacute;");
                            continue;
                        }
                    case 0xd4:
                        {
                            builder.Append("&Ocirc;");
                            continue;
                        }
                    case 0xd5:
                        {
                            builder.Append("&Otilde;");
                            continue;
                        }
                    case 0xd6:
                        {
                            builder.Append("&Ouml;");
                            continue;
                        }
                    case 0xd7:
                        {
                            builder.Append("&times;");
                            continue;
                        }
                    case 0xd8:
                        {
                            builder.Append("&Oslash;");
                            continue;
                        }
                    case 0xd9:
                        {
                            builder.Append("&Ugrave;");
                            continue;
                        }
                    case 0xda:
                        {
                            builder.Append("&Uacute;");
                            continue;
                        }
                    case 0xdb:
                        {
                            builder.Append("&Ucirc;");
                            continue;
                        }
                    case 220:
                        {
                            builder.Append("&Uuml;");
                            continue;
                        }
                    case 0xdd:
                        {
                            builder.Append("&Yacute;");
                            continue;
                        }
                    case 0xde:
                        {
                            builder.Append("&THORN;");
                            continue;
                        }
                    case 0xdf:
                        {
                            builder.Append("&szlig;");
                            continue;
                        }
                    case 0xe0:
                        {
                            builder.Append("&agrave;");
                            continue;
                        }
                    case 0xe1:
                        {
                            builder.Append("&aacute;");
                            continue;
                        }
                    case 0xe2:
                        {
                            builder.Append("&acirc;");
                            continue;
                        }
                    case 0xe3:
                        {
                            builder.Append("&atilde;");
                            continue;
                        }
                    case 0xe4:
                        {
                            builder.Append("&auml;");
                            continue;
                        }
                    case 0xe5:
                        {
                            builder.Append("&aring;");
                            continue;
                        }
                    case 230:
                        {
                            builder.Append("&aelig;");
                            continue;
                        }
                    case 0xe7:
                        {
                            builder.Append("&ccedil;");
                            continue;
                        }
                    case 0xe8:
                        {
                            builder.Append("&egrave;");
                            continue;
                        }
                    case 0xe9:
                        {
                            builder.Append("&eacute;");
                            continue;
                        }
                    case 0xea:
                        {
                            builder.Append("&ecirc;");
                            continue;
                        }
                    case 0xeb:
                        {
                            builder.Append("&euml;");
                            continue;
                        }
                    case 0xec:
                        {
                            builder.Append("&igrave;");
                            continue;
                        }
                    case 0xed:
                        {
                            builder.Append("&iacute;");
                            continue;
                        }
                    case 0xee:
                        {
                            builder.Append("&icirc;");
                            continue;
                        }
                    case 0xef:
                        {
                            builder.Append("&iuml;");
                            continue;
                        }
                    case 240:
                        {
                            builder.Append("&eth;");
                            continue;
                        }
                    case 0xf1:
                        {
                            builder.Append("&ntilde;");
                            continue;
                        }
                    case 0xf2:
                        {
                            builder.Append("&ograve;");
                            continue;
                        }
                    case 0xf3:
                        {
                            builder.Append("&oacute;");
                            continue;
                        }
                    case 0xf4:
                        {
                            builder.Append("&ocirc;");
                            continue;
                        }
                    case 0xf5:
                        {
                            builder.Append("&otilde;");
                            continue;
                        }
                    case 0xf6:
                        {
                            builder.Append("&ouml;");
                            continue;
                        }
                    case 0xf7:
                        {
                            builder.Append("&divide;");
                            continue;
                        }
                    case 0xf8:
                        {
                            builder.Append("&oslash;");
                            continue;
                        }
                    case 0xf9:
                        {
                            builder.Append("&ugrave;");
                            continue;
                        }
                    case 250:
                        {
                            builder.Append("&uacute;");
                            continue;
                        }
                    case 0xfb:
                        {
                            builder.Append("&ucirc;");
                            continue;
                        }
                    case 0xfc:
                        {
                            builder.Append("&uuml;");
                            continue;
                        }
                    case 0xfd:
                        {
                            builder.Append("&yacute;");
                            continue;
                        }
                    case 0xfe:
                        {
                            builder.Append("&thorn;");
                            continue;
                        }
                    case 0xff:
                        {
                            builder.Append("&yuml;");
                            continue;
                        }
                    case 0x152:
                        {
                            builder.Append("&OElig;");
                            continue;
                        }
                    case 0x153:
                        {
                            builder.Append("&oelig;");
                            continue;
                        }
                    case 0x160:
                        {
                            builder.Append("&Scaron;");
                            continue;
                        }
                    case 0x161:
                        {
                            builder.Append("&scaron;");
                            continue;
                        }
                    case 0x178:
                        {
                            builder.Append("&Yuml;");
                            continue;
                        }
                    case 0x192:
                        {
                            builder.Append("&fnof;");
                            continue;
                        }
                    case 0x391:
                        {
                            builder.Append("&Alpha;");
                            continue;
                        }
                    case 0x392:
                        {
                            builder.Append("&Beta;");
                            continue;
                        }
                    case 0x393:
                        {
                            builder.Append("&Gamma;");
                            continue;
                        }
                    case 0x394:
                        {
                            builder.Append("&Delta;");
                            continue;
                        }
                    case 0x395:
                        {
                            builder.Append("&Epsilon;");
                            continue;
                        }
                    case 0x396:
                        {
                            builder.Append("&Zeta;");
                            continue;
                        }
                    case 0x397:
                        {
                            builder.Append("&Eta;");
                            continue;
                        }
                    case 920:
                        {
                            builder.Append("&Theta;");
                            continue;
                        }
                    case 0x399:
                        {
                            builder.Append("&Iota;");
                            continue;
                        }
                    case 0x39a:
                        {
                            builder.Append("&Kappa;");
                            continue;
                        }
                    case 0x39b:
                        {
                            builder.Append("&Lambda;");
                            continue;
                        }
                    case 0x39c:
                        {
                            builder.Append("&Mu;");
                            continue;
                        }
                    case 0x39d:
                        {
                            builder.Append("&Nu;");
                            continue;
                        }
                    case 0x39e:
                        {
                            builder.Append("&Xi;");
                            continue;
                        }
                    case 0x39f:
                        {
                            builder.Append("&Omicron;");
                            continue;
                        }
                    case 0x3a0:
                        {
                            builder.Append("&Pi;");
                            continue;
                        }
                    case 0x3a1:
                        {
                            builder.Append("&Rho;");
                            continue;
                        }
                    case 0x3a3:
                        {
                            builder.Append("&Sigma;");
                            continue;
                        }
                    case 0x3a4:
                        {
                            builder.Append("&Tau;");
                            continue;
                        }
                    case 0x3a5:
                        {
                            builder.Append("&Upsilon;");
                            continue;
                        }
                    case 0x3a6:
                        {
                            builder.Append("&Phi;");
                            continue;
                        }
                    case 0x3a7:
                        {
                            builder.Append("&Chi;");
                            continue;
                        }
                    case 0x3a8:
                        {
                            builder.Append("&Psi;");
                            continue;
                        }
                    case 0x3a9:
                        {
                            builder.Append("&Omega;");
                            continue;
                        }
                    case 0x3b1:
                        {
                            builder.Append("&alpha;");
                            continue;
                        }
                    case 0x3b2:
                        {
                            builder.Append("&beta;");
                            continue;
                        }
                    case 0x3b3:
                        {
                            builder.Append("&gamma;");
                            continue;
                        }
                    case 0x3b4:
                        {
                            builder.Append("&delta;");
                            continue;
                        }
                    case 0x3b5:
                        {
                            builder.Append("&epsilon;");
                            continue;
                        }
                    case 950:
                        {
                            builder.Append("&zeta;");
                            continue;
                        }
                    case 0x3b7:
                        {
                            builder.Append("&eta;");
                            continue;
                        }
                    case 0x3b8:
                        {
                            builder.Append("&theta;");
                            continue;
                        }
                    case 0x3b9:
                        {
                            builder.Append("&iota;");
                            continue;
                        }
                    case 0x3ba:
                        {
                            builder.Append("&kappa;");
                            continue;
                        }
                    case 0x3bb:
                        {
                            builder.Append("&lambda;");
                            continue;
                        }
                    case 0x3bc:
                        {
                            builder.Append("&mu;");
                            continue;
                        }
                    case 0x3bd:
                        {
                            builder.Append("&nu;");
                            continue;
                        }
                    case 0x3be:
                        {
                            builder.Append("&xi;");
                            continue;
                        }
                    case 0x3bf:
                        {
                            builder.Append("&omicron;");
                            continue;
                        }
                    case 960:
                        {
                            builder.Append("&pi;");
                            continue;
                        }
                    case 0x3c1:
                        {
                            builder.Append("&rho;");
                            continue;
                        }
                    case 0x3c2:
                        {
                            builder.Append("&sigmaf;");
                            continue;
                        }
                    case 0x3c3:
                        {
                            builder.Append("&sigma;");
                            continue;
                        }
                    case 0x3c4:
                        {
                            builder.Append("&tau;");
                            continue;
                        }
                    case 0x3c5:
                        {
                            builder.Append("&upsilon;");
                            continue;
                        }
                    case 0x3c6:
                        {
                            builder.Append("&phi;");
                            continue;
                        }
                    case 0x3c7:
                        {
                            builder.Append("&chi;");
                            continue;
                        }
                    case 0x3c8:
                        {
                            builder.Append("&psi;");
                            continue;
                        }
                    case 0x3c9:
                        {
                            builder.Append("&omega;");
                            continue;
                        }
                    case 0x3d1:
                        {
                            builder.Append("&thetasym;");
                            continue;
                        }
                    case 0x3d2:
                        {
                            builder.Append("&upsih;");
                            continue;
                        }
                    case 0x3d6:
                        {
                            builder.Append("&piv;");
                            continue;
                        }
                    case 0x2dc:
                        {
                            builder.Append("&tilde;");
                            continue;
                        }
                    case 710:
                        {
                            builder.Append("&circ;");
                            continue;
                        }
                    case 0x2003:
                        {
                            builder.Append("&emsp;");
                            continue;
                        }
                    case 0x2009:
                        {
                            builder.Append("&thinsp;");
                            continue;
                        }
                    case 0x200c:
                        {
                            builder.Append("&zwnj;");
                            continue;
                        }
                    case 0x200d:
                        {
                            builder.Append("&zwj;");
                            continue;
                        }
                    case 0x200e:
                        {
                            builder.Append("&lrm;");
                            continue;
                        }
                    case 0x200f:
                        {
                            builder.Append("&rlm;");
                            continue;
                        }
                    case 0x2013:
                        {
                            builder.Append("&ndash;");
                            continue;
                        }
                    case 0x2014:
                        {
                            builder.Append("&mdash;");
                            continue;
                        }
                    case 0x2018:
                        {
                            builder.Append("&lsquo;");
                            continue;
                        }
                    case 0x2019:
                        {
                            builder.Append("&rsquo;");
                            continue;
                        }
                    case 0x201a:
                        {
                            builder.Append("&sbquo;");
                            continue;
                        }
                    case 0x201c:
                        {
                            builder.Append("&ldquo;");
                            continue;
                        }
                    case 0x201d:
                        {
                            builder.Append("&rdquo;");
                            continue;
                        }
                    case 0x201e:
                        {
                            builder.Append("&bdquo;");
                            continue;
                        }
                    case 0x2020:
                        {
                            builder.Append("&dagger;");
                            continue;
                        }
                    case 0x2021:
                        {
                            builder.Append("&Dagger;");
                            continue;
                        }
                    case 0x2022:
                        {
                            builder.Append("&bull;");
                            continue;
                        }
                    case 0x2026:
                        {
                            builder.Append("&hellip;");
                            continue;
                        }
                    case 0x2030:
                        {
                            builder.Append("&permil;");
                            continue;
                        }
                    case 0x2032:
                        {
                            builder.Append("&prime;");
                            continue;
                        }
                    case 0x2033:
                        {
                            builder.Append("&Prime;");
                            continue;
                        }
                    case 0x2039:
                        {
                            builder.Append("&lsaquo;");
                            continue;
                        }
                    case 0x203a:
                        {
                            builder.Append("&rsaquo;");
                            continue;
                        }
                    case 0x203e:
                        {
                            builder.Append("&oline;");
                            continue;
                        }
                    case 0x2044:
                        {
                            builder.Append("&fras1;");
                            continue;
                        }
                    case 0x2111:
                        {
                            builder.Append("&image;");
                            continue;
                        }
                    case 0x2118:
                        {
                            builder.Append("&weierp;");
                            continue;
                        }
                    case 0x211c:
                        {
                            builder.Append("&real;");
                            continue;
                        }
                    case 0x2122:
                        {
                            builder.Append("&trade;");
                            continue;
                        }
                    case 0x2190:
                        {
                            builder.Append("&larr;");
                            continue;
                        }
                    case 0x2191:
                        {
                            builder.Append("&uarr;");
                            continue;
                        }
                    case 0x2192:
                        {
                            builder.Append("&rarr;");
                            continue;
                        }
                    case 0x2193:
                        {
                            builder.Append("&darr;");
                            continue;
                        }
                    case 0x2194:
                        {
                            builder.Append("&harr;");
                            continue;
                        }
                    case 0x2135:
                        {
                            builder.Append("&alefsym;");
                            continue;
                        }
                    case 0x21d0:
                        {
                            builder.Append("&lArr;");
                            continue;
                        }
                    case 0x21d1:
                        {
                            builder.Append("&uArr;");
                            continue;
                        }
                    case 0x21d2:
                        {
                            builder.Append("&rArr;");
                            continue;
                        }
                    case 0x21d3:
                        {
                            builder.Append("&dArr;");
                            continue;
                        }
                    case 0x21d4:
                        {
                            builder.Append("&hArr;");
                            continue;
                        }
                    case 0x21b5:
                        {
                            builder.Append("&crarr;");
                            continue;
                        }
                    case 0x2200:
                        {
                            builder.Append("&forall;");
                            continue;
                        }
                    case 0x2202:
                        {
                            builder.Append("&part;");
                            continue;
                        }
                    case 0x2203:
                        {
                            builder.Append("&exist;");
                            continue;
                        }
                    case 0x2205:
                        {
                            builder.Append("&empty;");
                            continue;
                        }
                    case 0x2207:
                        {
                            builder.Append("&nabla;");
                            continue;
                        }
                    case 0x2208:
                        {
                            builder.Append("&isin;");
                            continue;
                        }
                    case 0x2209:
                        {
                            builder.Append("&notin;");
                            continue;
                        }
                    case 0x220b:
                        {
                            builder.Append("&ni;");
                            continue;
                        }
                    case 0x220f:
                        {
                            builder.Append("&prod;");
                            continue;
                        }
                    case 0x2211:
                        {
                            builder.Append("&sum;");
                            continue;
                        }
                    case 0x2212:
                        {
                            builder.Append("&minus;");
                            continue;
                        }
                    case 0x221a:
                        {
                            builder.Append("&radic;");
                            continue;
                        }
                    case 0x221d:
                        {
                            builder.Append("&prop;");
                            continue;
                        }
                    case 0x221e:
                        {
                            builder.Append("&infin;");
                            continue;
                        }
                    case 0x2220:
                        {
                            builder.Append("&ang;");
                            continue;
                        }
                    case 0x2217:
                        {
                            builder.Append("&lowast;");
                            continue;
                        }
                    case 0x2227:
                        {
                            builder.Append("&and;");
                            continue;
                        }
                    case 0x2228:
                        {
                            builder.Append("&or;");
                            continue;
                        }
                    case 0x2229:
                        {
                            builder.Append("&cap;");
                            continue;
                        }
                    case 0x222a:
                        {
                            builder.Append("&cup;");
                            continue;
                        }
                    case 0x222b:
                        {
                            builder.Append("&int;");
                            continue;
                        }
                    case 0x2234:
                        {
                            builder.Append("&there4;");
                            continue;
                        }
                    case 0x223c:
                        {
                            builder.Append("&sim;");
                            continue;
                        }
                    case 0x2245:
                        {
                            builder.Append("&cong;");
                            continue;
                        }
                    case 0x2248:
                        {
                            builder.Append("&asymp;");
                            continue;
                        }
                    case 0x2260:
                        {
                            builder.Append("&ne;");
                            continue;
                        }
                    case 0x2261:
                        {
                            builder.Append("&equiv;");
                            continue;
                        }
                    case 0x2264:
                        {
                            builder.Append("&le;");
                            continue;
                        }
                    case 0x2265:
                        {
                            builder.Append("&ge;");
                            continue;
                        }
                    case 0x2282:
                        {
                            builder.Append("&sub;");
                            continue;
                        }
                    case 0x2283:
                        {
                            builder.Append("&sup;");
                            continue;
                        }
                    case 0x2284:
                        {
                            builder.Append("&nsub;");
                            continue;
                        }
                    case 0x2286:
                        {
                            builder.Append("&sube;");
                            continue;
                        }
                    case 0x2287:
                        {
                            builder.Append("&supe;");
                            continue;
                        }
                    case 0x2295:
                        {
                            builder.Append("&oplus;");
                            continue;
                        }
                    case 0x2297:
                        {
                            builder.Append("&otimes;");
                            continue;
                        }
                    case 0x2308:
                        {
                            builder.Append("&lceil;");
                            continue;
                        }
                    case 0x2309:
                        {
                            builder.Append("&rceil;");
                            continue;
                        }
                    case 0x230a:
                        {
                            builder.Append("&lfloor;");
                            continue;
                        }
                    case 0x230b:
                        {
                            builder.Append("&rfloor;");
                            continue;
                        }
                    case 0x22c5:
                        {
                            builder.Append("&sdot;");
                            continue;
                        }
                    case 0x22a5:
                        {
                            builder.Append("&perp;");
                            continue;
                        }
                    case 0x2329:
                        {
                            builder.Append("&lang;");
                            continue;
                        }
                    case 0x232a:
                        {
                            builder.Append("&rang;");
                            continue;
                        }
                    case 0x25ca:
                        {
                            builder.Append("&loz;");
                            continue;
                        }
                    case 0x2660:
                        {
                            builder.Append("&spades;");
                            continue;
                        }
                    case 0x2663:
                        {
                            builder.Append("&clubs;");
                            continue;
                        }
                    case 0x2665:
                        {
                            builder.Append("&hearts;");
                            continue;
                        }
                    case 0x2666:
                        {
                            builder.Append("&diams;");
                            continue;
                        }
                }
                if (i <= 0x7f)
                {
                    builder.Append((char)i);
                }
                else
                {
                    builder.Append("&#" + i + ";");
                }
            }
            return builder.ToString();
        }

        public static string DecodeHexValue(string s)
        {
            System.Collections.Hashtable entities = new System.Collections.Hashtable();
            entities.Add("nbsp", '\u00A0');
            entities.Add("iexcl", '\u00A1');
            entities.Add("cent", '\u00A2');
            entities.Add("pound", '\u00A3');
            entities.Add("curren", '\u00A4');
            entities.Add("yen", '\u00A5');
            entities.Add("brvbar", '\u00A6');
            entities.Add("sect", '\u00A7');
            entities.Add("uml", '\u00A8');
            entities.Add("copy", '\u00A9');
            entities.Add("ordf", '\u00AA');
            entities.Add("laquo", '\u00AB');
            entities.Add("not", '\u00AC');
            entities.Add("shy", '\u00AD');
            entities.Add("reg", '\u00AE');
            entities.Add("macr", '\u00AF');
            entities.Add("deg", '\u00B0');
            entities.Add("plusmn", '\u00B1');
            entities.Add("sup2", '\u00B2');
            entities.Add("sup3", '\u00B3');
            entities.Add("acute", '\u00B4');
            entities.Add("micro", '\u00B5');
            entities.Add("para", '\u00B6');
            entities.Add("middot", '\u00B7');
            entities.Add("cedil", '\u00B8');
            entities.Add("sup1", '\u00B9');
            entities.Add("ordm", '\u00BA');
            entities.Add("raquo", '\u00BB');
            entities.Add("frac14", '\u00BC');
            entities.Add("frac12", '\u00BD');
            entities.Add("frac34", '\u00BE');
            entities.Add("iquest", '\u00BF');
            entities.Add("Agrave", '\u00C0');
            entities.Add("Aacute", '\u00C1');
            entities.Add("Acirc", '\u00C2');
            entities.Add("Atilde", '\u00C3');
            entities.Add("Auml", '\u00C4');
            entities.Add("Aring", '\u00C5');
            entities.Add("AElig", '\u00C6');
            entities.Add("Ccedil", '\u00C7');
            entities.Add("Egrave", '\u00C8');
            entities.Add("Eacute", '\u00C9');
            entities.Add("Ecirc", '\u00CA');
            entities.Add("Euml", '\u00CB');
            entities.Add("Igrave", '\u00CC');
            entities.Add("Iacute", '\u00CD');
            entities.Add("Icirc", '\u00CE');
            entities.Add("Iuml", '\u00CF');
            entities.Add("ETH", '\u00D0');
            entities.Add("Ntilde", '\u00D1');
            entities.Add("Ograve", '\u00D2');
            entities.Add("Oacute", '\u00D3');
            entities.Add("Ocirc", '\u00D4');
            entities.Add("Otilde", '\u00D5');
            entities.Add("Ouml", '\u00D6');
            entities.Add("times", '\u00D7');
            entities.Add("Oslash", '\u00D8');
            entities.Add("Ugrave", '\u00D9');
            entities.Add("Uacute", '\u00DA');
            entities.Add("Ucirc", '\u00DB');
            entities.Add("Uuml", '\u00DC');
            entities.Add("Yacute", '\u00DD');
            entities.Add("THORN", '\u00DE');
            entities.Add("szlig", '\u00DF');
            entities.Add("agrave", '\u00E0');
            entities.Add("aacute", '\u00E1');
            entities.Add("acirc", '\u00E2');
            entities.Add("atilde", '\u00E3');
            entities.Add("auml", '\u00E4');
            entities.Add("aring", '\u00E5');
            entities.Add("aelig", '\u00E6');
            entities.Add("ccedil", '\u00E7');
            entities.Add("egrave", '\u00E8');
            entities.Add("eacute", '\u00E9');
            entities.Add("ecirc", '\u00EA');
            entities.Add("euml", '\u00EB');
            entities.Add("igrave", '\u00EC');
            entities.Add("iacute", '\u00ED');
            entities.Add("icirc", '\u00EE');
            entities.Add("iuml", '\u00EF');
            entities.Add("eth", '\u00F0');
            entities.Add("ntilde", '\u00F1');
            entities.Add("ograve", '\u00F2');
            entities.Add("oacute", '\u00F3');
            entities.Add("ocirc", '\u00F4');
            entities.Add("otilde", '\u00F5');
            entities.Add("ouml", '\u00F6');
            entities.Add("divide", '\u00F7');
            entities.Add("oslash", '\u00F8');
            entities.Add("ugrave", '\u00F9');
            entities.Add("uacute", '\u00FA');
            entities.Add("ucirc", '\u00FB');
            entities.Add("uuml", '\u00FC');
            entities.Add("yacute", '\u00FD');
            entities.Add("thorn", '\u00FE');
            entities.Add("yuml", '\u00FF');
            entities.Add("fnof", '\u0192');
            entities.Add("Alpha", '\u0391');
            entities.Add("Beta", '\u0392');
            entities.Add("Gamma", '\u0393');
            entities.Add("Delta", '\u0394');
            entities.Add("Epsilon", '\u0395');
            entities.Add("Zeta", '\u0396');
            entities.Add("Eta", '\u0397');
            entities.Add("Theta", '\u0398');
            entities.Add("Iota", '\u0399');
            entities.Add("Kappa", '\u039A');
            entities.Add("Lambda", '\u039B');
            entities.Add("Mu", '\u039C');
            entities.Add("Nu", '\u039D');
            entities.Add("Xi", '\u039E');
            entities.Add("Omicron", '\u039F');
            entities.Add("Pi", '\u03A0');
            entities.Add("Rho", '\u03A1');
            entities.Add("Sigma", '\u03A3');
            entities.Add("Tau", '\u03A4');
            entities.Add("Upsilon", '\u03A5');
            entities.Add("Phi", '\u03A6');
            entities.Add("Chi", '\u03A7');
            entities.Add("Psi", '\u03A8');
            entities.Add("Omega", '\u03A9');
            entities.Add("alpha", '\u03B1');
            entities.Add("beta", '\u03B2');
            entities.Add("gamma", '\u03B3');
            entities.Add("delta", '\u03B4');
            entities.Add("epsilon", '\u03B5');
            entities.Add("zeta", '\u03B6');
            entities.Add("eta", '\u03B7');
            entities.Add("theta", '\u03B8');
            entities.Add("iota", '\u03B9');
            entities.Add("kappa", '\u03BA');
            entities.Add("lambda", '\u03BB');
            entities.Add("mu", '\u03BC');
            entities.Add("nu", '\u03BD');
            entities.Add("xi", '\u03BE');
            entities.Add("omicron", '\u03BF');
            entities.Add("pi", '\u03C0');
            entities.Add("rho", '\u03C1');
            entities.Add("sigmaf", '\u03C2');
            entities.Add("sigma", '\u03C3');
            entities.Add("tau", '\u03C4');
            entities.Add("upsilon", '\u03C5');
            entities.Add("phi", '\u03C6');
            entities.Add("chi", '\u03C7');
            entities.Add("psi", '\u03C8');
            entities.Add("omega", '\u03C9');
            entities.Add("thetasym", '\u03D1');
            entities.Add("upsih", '\u03D2');
            entities.Add("piv", '\u03D6');
            entities.Add("bull", '\u2022');
            entities.Add("hellip", '\u2026');
            entities.Add("prime", '\u2032');
            entities.Add("Prime", '\u2033');
            entities.Add("oline", '\u203E');
            entities.Add("frasl", '\u2044');
            entities.Add("weierp", '\u2118');
            entities.Add("image", '\u2111');
            entities.Add("real", '\u211C');
            entities.Add("trade", '\u2122');
            entities.Add("alefsym", '\u2135');
            entities.Add("larr", '\u2190');
            entities.Add("uarr", '\u2191');
            entities.Add("rarr", '\u2192');
            entities.Add("darr", '\u2193');
            entities.Add("harr", '\u2194');
            entities.Add("crarr", '\u21B5');
            entities.Add("lArr", '\u21D0');
            entities.Add("uArr", '\u21D1');
            entities.Add("rArr", '\u21D2');
            entities.Add("dArr", '\u21D3');
            entities.Add("hArr", '\u21D4');
            entities.Add("forall", '\u2200');
            entities.Add("part", '\u2202');
            entities.Add("exist", '\u2203');
            entities.Add("empty", '\u2205');
            entities.Add("nabla", '\u2207');
            entities.Add("isin", '\u2208');
            entities.Add("notin", '\u2209');
            entities.Add("ni", '\u220B');
            entities.Add("prod", '\u220F');
            entities.Add("sum", '\u2211');
            entities.Add("minus", '\u2212');
            entities.Add("lowast", '\u2217');
            entities.Add("radic", '\u221A');
            entities.Add("prop", '\u221D');
            entities.Add("infin", '\u221E');
            entities.Add("ang", '\u2220');
            entities.Add("and", '\u2227');
            entities.Add("or", '\u2228');
            entities.Add("cap", '\u2229');
            entities.Add("cup", '\u222A');
            entities.Add("int", '\u222B');
            entities.Add("there4", '\u2234');
            entities.Add("sim", '\u223C');
            entities.Add("cong", '\u2245');
            entities.Add("asymp", '\u2248');
            entities.Add("ne", '\u2260');
            entities.Add("equiv", '\u2261');
            entities.Add("le", '\u2264');
            entities.Add("ge", '\u2265');
            entities.Add("sub", '\u2282');
            entities.Add("sup", '\u2283');
            entities.Add("nsub", '\u2284');
            entities.Add("sube", '\u2286');
            entities.Add("supe", '\u2287');
            entities.Add("oplus", '\u2295');
            entities.Add("otimes", '\u2297');
            entities.Add("perp", '\u22A5');
            entities.Add("sdot", '\u22C5');
            entities.Add("lceil", '\u2308');
            entities.Add("rceil", '\u2309');
            entities.Add("lfloor", '\u230A');
            entities.Add("rfloor", '\u230B');
            entities.Add("lang", '\u2329');
            entities.Add("rang", '\u232A');
            entities.Add("loz", '\u25CA');
            entities.Add("spades", '\u2660');
            entities.Add("clubs", '\u2663');
            entities.Add("hearts", '\u2665');
            entities.Add("diams", '\u2666');
            entities.Add("quot", '\u0022');
            entities.Add("amp", '\u0026');
            entities.Add("lt", '\u003C');
            entities.Add("gt", '\u003E');
            entities.Add("OElig", '\u0152');
            entities.Add("oelig", '\u0153');
            entities.Add("Scaron", '\u0160');
            entities.Add("scaron", '\u0161');
            entities.Add("Yuml", '\u0178');
            entities.Add("circ", '\u02C6');
            entities.Add("tilde", '\u02DC');
            entities.Add("ensp", '\u2002');
            entities.Add("emsp", '\u2003');
            entities.Add("thinsp", '\u2009');
            entities.Add("zwnj", '\u200C');
            entities.Add("zwj", '\u200D');
            entities.Add("lrm", '\u200E');
            entities.Add("rlm", '\u200F');
            entities.Add("ndash", '\u2013');
            entities.Add("mdash", '\u2014');
            entities.Add("lsquo", '\u2018');
            entities.Add("rsquo", '\u2019');
            entities.Add("sbquo", '\u201A');
            entities.Add("ldquo", '\u201C');
            entities.Add("rdquo", '\u201D');
            entities.Add("bdquo", '\u201E');
            entities.Add("dagger", '\u2020');
            entities.Add("Dagger", '\u2021');
            entities.Add("permil", '\u2030');
            entities.Add("lsaquo", '\u2039');
            entities.Add("rsaquo", '\u203A');
            entities.Add("euro", '\u20AC');
            if (s == null)
                throw new ArgumentNullException("s");

            bool insideEntity = false;
            string entity = String.Empty;
            StringBuilder output = new StringBuilder();

            foreach (char c in s)
            {
                switch (c)
                {
                    case '&':
                        output.Append(entity);
                        entity = "&";

                        insideEntity = true;
                        break;
                    case ';':
                        if (!insideEntity)
                        {
                            output.Append(c);
                            break;
                        }

                        entity += c;
                        int length = entity.Length;
                        if (length >= 2 && entity[1] == '#' && entity[2] != ';')
                            entity = ((char)Int32.Parse(entity.Substring(2,
                            entity.Length - 3))).ToString();
                        else if (length > 1 && entities.ContainsKey(entity.Substring(1, entity.Length - 2)))

                            entity = entities[entity.Substring(1, entity.Length - 2)].ToString();

                        output.Append(entity);
                        entity = String.Empty;

                        insideEntity = false;
                        break;
                    default:
                        if (insideEntity)
                            entity += c;
                        else
                            output.Append(c);
                        break;
                }
            }
            output.Append(entity);
            return output.ToString();
        }
    }
}

