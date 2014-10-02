using UnityEngine;
using System.Collections.Generic;
using System;
using Prime31;



public class FacebookBaseDTO
{
	public override string ToString()
	{
		return JsonFormatter.prettyPrint( Json.encode( this ) ) ?? string.Empty;
	}
}


public class FacebookFriendsResult : FacebookBaseDTO
{
	public List<FacebookFriend> data = new List<FacebookFriend>();
	public FacebookResultsPaging paging;
}


public class FacebookResultsPaging : FacebookBaseDTO
{
	public string next;
	public string previous;
}


public class FacebookFriend : FacebookBaseDTO
{
	public string name;
	public string id;
}


public class FacebookMeResult : FacebookBaseDTO
{
	public class FacebookMeHometown : FacebookBaseDTO
	{
		public string id;
		public string name;
	}
	
	public class FacebookMeLocation : FacebookBaseDTO
	{
		public string id;
		public string name;
	}
	
	public string id;
	public string name;
	public string first_name;
	public string last_name;
	public string link;
	public string username;
	public FacebookMeHometown hometown;
	public FacebookMeLocation location;
	public string gender;
	public string email;
	public int timezone;
	public string locale;
	public bool verified;
	public DateTime updated_time;
}

/*
	"hometown":
	{
		"id": "104079956295173", 		
		"name": "Morris Plains, New Jersey"
	}, 	
	"location":
	{
		"id": "109680939058152", 		
		"name": "Encinitas, California"
	}, 	
*/