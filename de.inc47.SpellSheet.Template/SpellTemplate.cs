using System;
using de.inc47.Spells;
using de.inc47.Spells.Enumerations;
using de.inc47.SpellSheet.Render;
using de.inc47.SpellSheet.Render.Enum;

namespace de.inc47.SpellSheet.Template
{
  public class SpellTemplate : ITemplate<Tuple<ISpell, ICharacterInformation>>
  {
    public IBlock Apply(Tuple<ISpell, ICharacterInformation> data)
    {
      var spell = data.Item1;
      var info = data.Item2;
      var root = new Block();

      root.Children.Add(RenderEigenschaften(info));

      return root;
    }

    private Block RenderEigenschaften(ICharacterInformation character)
    {
      var block = new Block();
      foreach (Eigenschaft e in Enum.GetValues(typeof(Eigenschaft)))
      {
        var eigenschaftsBlock = new Block();
        var label = new Text(56, 0, 2, 1, e.ToString(), TextStyle.Label);
        var text = new Text(57,0,2,1,character.GetEigenschaft(e).ToString(),TextStyle.Default);
        eigenschaftsBlock.Children.Add(label);
        eigenschaftsBlock.Children.Add(text);
        block.Children.Add(eigenschaftsBlock);
      }
      return block;
    }
  }
}