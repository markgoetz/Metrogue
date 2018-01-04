[System.Serializable]
public struct IntRect {
	public int xMin;
	public int xMax;
	public int yMin;
	public int yMax;

	public IntRect(int xMin, int xMax, int yMin, int yMax) {
		this.xMin = xMin;
		this.xMax = xMax;
		this.yMin = yMin;
		this.yMax = yMax;
	}
}