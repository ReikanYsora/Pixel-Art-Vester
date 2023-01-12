using System.Collections.Generic;

public class PaintInventory
{
	#region PROPERTY
	public int Index { set; get; }

	public CMYColor Color { set; get; }

	public int Quantity { set; get; }

	public bool Infinite { set; get; }

    public List<CMYColor> ToCreate { set; get; }
    #endregion
}
