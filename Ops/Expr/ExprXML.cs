using PSI;
using System.Xml.Linq;

namespace Ops.Expr {
   public class ExprXML : Visitor<XElement> {
      public ExprXML (Dictionary<string, NType> dict) => mDict = dict;
      public Dictionary<string, NType> mDict;
      public override XElement Visit (NLiteral literal) {
         XElement child = new ("literal");
         child.SetAttributeValue ("Value", literal.Value.Text);
         child.SetAttributeValue ("Type", literal.Type);
         return child;
      }

      public override XElement Visit (NIdentifier ident) {
         XElement child = new ("ident");
         child.SetAttributeValue ("Name", ident.Name.Text);
         child.SetAttributeValue ("Type", mDict[ident.Name.Text]);
         return child;
      }

      public override XElement Visit (NUnary unary) {
         var d = unary.Expr.Accept (this);
         XElement child = new ("Unary");
         child.SetAttributeValue ("Value", unary.Op.Kind);
         child.SetAttributeValue ("Type", d.NodeType);
         return child;
      }

      public override XElement Visit (NBinary binary) {
         XElement child = new ("Binary");
         child.SetAttributeValue ("Op", binary.Op.Kind);
         child.SetAttributeValue ("Type", binary.Type);
         XElement a = binary.Left.Accept (this), b = binary.Right.Accept (this);
         child.Add (a);
         child.Add (b);
         return child;
      }
      public void SaveAs (string path, XElement element) {
         File.WriteAllText (path, element.ToString ());
      }
   }
}