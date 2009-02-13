namespace ccnet.labeller.tests
{
	public class SvnRevisionLabellerStub : SvnRevisionLabeller
	{
		public void SetRevision(int svnRevision)
		{
			_revision = svnRevision;
		}

		protected override int GetRevision()
		{
			return _revision;
		}

		private int _revision;
	}
}
