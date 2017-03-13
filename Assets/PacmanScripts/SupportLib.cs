using UnityEngine;
using System.Collections;

public enum Icons{
	Ready,Go,DirectionBtn,DirectionDot,SwopBtn,SwopNew,LeftBtn,RightBtn
}

public static class SupportLib {

	public static void SetActiveRecursively(GameObject rootObject, bool active){
		rootObject.SetActive(active);
		foreach (Transform childTransform in rootObject.transform){
			SetActiveRecursively(childTransform.gameObject, active);
		}
	}

	private static float scaleWidthiPad(float w){
		return w/2048*Screen.width;
	}

	private static float scaleWidthiPhoneLong(float w){
		return w/1136*Screen.width;
	}

	private static float scaleWidthiPhoneShort(float w){
		return w/960*Screen.width;
	}
		
	private static float scaleHeightiPad(float w){
		return w/1536*Screen.width;
	}

	private static float scaleHeightiPhoneLong(float w){
		return w/640*Screen.width;
	}

	private static float scaleHeightiPhoneShort(float w){
		return w/640*Screen.width;
	}

	public static Rect fixRectResolutioniPad( Rect rect ){
		return new 
			Rect( rect.x/2048*Screen.width, 	rect.y/1536*Screen.height, 
			      rect.width/2048*Screen.width, rect.height/1536*Screen.height );
	}

	public static Rect fixRectResolutioniPhoneLong( Rect rect ){
		return new 
			Rect( rect.x/1136*Screen.width, 	rect.y/640*Screen.height, 
			      rect.width/1136*Screen.width, rect.height/640*Screen.height );
	} 

	public static Rect fixRectResolutioniPhoneShort( Rect rect ){
		return new 
			Rect( rect.x/960*Screen.width, 		rect.y/640*Screen.height, 
			      rect.width/960*Screen.width, 	rect.height/640*Screen.height );
	}

	public static bool isIphone(){
		return !((UnityEngine.iOS.Device.generation.ToString()).IndexOf("iPad") > -1);
	}

	public static float scaleWidth(float w){
		int type = isIphone() ? 1 : 0;
		if( 1f * Screen.width / Screen.height > 1.5f )
			type *= 2;
		if(type == 0)
			return scaleWidthiPad(w);
		else if(type == 2)
			return scaleWidthiPhoneLong(w);
		return scaleWidthiPhoneShort(w);
	}

	public static float scaleHeight(float h){
		int type = isIphone() ? 1 : 0;
		if( 1f * Screen.width / Screen.height > 1.5f )
			type *= 2;
		if(type == 0)
			return scaleHeightiPad(h);
		else if(type == 2)
			return scaleHeightiPhoneLong(h);
		return scaleHeightiPhoneShort(h);
	}

	public static Rect getIconRect( Icons icon ){

		int type = isIphone() ? 1 : 0;
		if( 1f * Screen.width / Screen.height > 1.5f )
			type *= 2;

		switch(icon){
		case Icons.Ready:
			if(type == 0)
				return fixRectResolutioniPad(new Rect(622,643,804,250));
			else if(type == 2)
				return fixRectResolutioniPhoneLong(new Rect(368,258,402,125));
			else return fixRectResolutioniPhoneShort(new Rect(280,258,402,125));
		case Icons.Go:
			if(type == 0)
				return fixRectResolutioniPad(new Rect(898,716,252,104));
			else if(type == 2)
				return fixRectResolutioniPhoneLong(new Rect(506,294,126,52));
			else return fixRectResolutioniPhoneShort(new Rect(418,294,126,52));
		case Icons.DirectionBtn:
			if(type == 0)
				return fixRectResolutioniPad(new Rect(52,1324,428,192));
			else if(type == 2) 
				return fixRectResolutioniPhoneLong(new Rect(26,524,332,96));
			else return fixRectResolutioniPhoneLong(new Rect(26,524,332,96));
		case Icons.DirectionDot:
			if(type == 0)
				return fixRectResolutioniPad(new Rect(52,1324,184,192));
			else if(type == 2) 
				return fixRectResolutioniPhoneLong(new Rect(26,524,92,96));
			else return fixRectResolutioniPhoneLong(new Rect(26,524,92,96));
		case Icons.SwopBtn:
			if(type == 0)
				return fixRectResolutioniPad(new Rect(1568,1392,428,124));
			else if(type == 2) 
				return fixRectResolutioniPhoneLong(new Rect(896,568,214,62));
			else return fixRectResolutioniPhoneShort(new Rect(720,568,214,62));
		case Icons.SwopNew:
			if(type == 0)
				return fixRectResolutioniPad(new Rect(1804,1324,192,192));
			else if(type == 2) 
				return fixRectResolutioniPhoneLong(new Rect(1014,524,96,96));
			else return fixRectResolutioniPhoneShort(new Rect(838,524,96,96));
		case Icons.LeftBtn:
			if(type == 0)
				return fixRectResolutioniPad(new Rect(52,1324,192,192));
			else if(type == 2) 
				return fixRectResolutioniPhoneLong(new Rect(26,524,96,96));
			else return fixRectResolutioniPhoneShort(new Rect(26,524,96,96));
		case Icons.RightBtn:
			if(type == 0)
				return fixRectResolutioniPad(new Rect(308,1324,192,192));
			else if(type == 2) 
				return fixRectResolutioniPhoneLong(new Rect(154,524,96,96));
			else return fixRectResolutioniPhoneShort(new Rect(154,524,96,96));
		}

		return new Rect();
	}
}
