public class BtSquence : BtNode {
	private BtNode[] children;
	private int currentIndex = 0;

	public BtSquence(params BtNode[] _children) {
		children = _children;
	}

	public override BtResult Run() {
		for(; currentIndex < children.Length; currentIndex++) {
			BtResult result = children[currentIndex].Run();

			switch(result) {
				case BtResult.failed:
					currentIndex = 0;
					return BtResult.failed;
				case BtResult.running:
					return BtResult.running;
				case BtResult.success:
					break;
			}
		}

		currentIndex = 0;
		return BtResult.success;
	}
}
