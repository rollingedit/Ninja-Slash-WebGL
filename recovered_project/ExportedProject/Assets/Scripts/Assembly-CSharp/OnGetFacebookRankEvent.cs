using System.Collections.Generic;

public class OnGetFacebookRankEvent : BaseEvent
{
	private List<FacebookUser> facebookUsers;

	public OnGetFacebookRankEvent(List<FacebookUser> facebookUsersIn)
	{
		facebookUsers = facebookUsersIn;
	}

	public override object GetData()
	{
		return facebookUsers;
	}
}
