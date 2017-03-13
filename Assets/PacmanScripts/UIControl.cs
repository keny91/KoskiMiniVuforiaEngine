using UnityEngine;
using System.Collections;

public class UIControl : MonoBehaviour {
	public bool useSlider = true;

	//intro
	public Texture scanningTex;
	public Texture readyTex;
	public Texture goTex;

	//ui
	public Texture directionBtn;
	public Texture directionDot;
	public Texture swopBtnOn;
	public Texture swopBtnOff;
	public Texture leftBtn;
	public Texture rightBtn;

	private Rect    readyRect;
	private Rect    goRect;
	private Rect    dotRect;
	private Rect    directionRect;
	private Rect 	swopRect;
	private Rect    leftRect;
	private Rect 	rightRect;

	private int mState = 0;
	private float mTime = 0f;
	private Color mIntroColor;
	private bool mSwopDown = false, mPrevSwopDown=false;
	private static UIControl mControl;

	private float mDirection = 0f;

	private Rect mActiveRect;
	private Texture mActiveTex;

	public static UIControl Instance{
		get{ return mControl; }
	}

	public void foundTarget(){
		if(mState > 0) return;
		mState = 1;
		mIntroColor.a = 0f;
		mTime = Time.time;
		mActiveTex = readyTex;
	}

	public float getDirectionFloat(){
		return mDirection;
	}
	public int getDirection(){
		if(Mathf.Abs(mDirection) < 0.1f)
			return 0;
		if(mDirection > 0f) return 1;
		return -1;
	}
	public bool getSwopBtn(){
		bool val = mSwopDown;
		mSwopDown = false;
		return val;
	}

	private float fract(float val){
		return (val - Mathf.Floor (val));
	}

	private bool handleTouch(float x, float y){
		Vector2 pos = new Vector2(x, Screen.height - y);
		if( directionRect.Contains(pos) ){
			Vector2 offset = pos - directionRect.center;
			mDirection = Mathf.Min(1f, 2f * offset.x / directionRect.width);
			dotRect.x = Mathf.Clamp( pos.x - dotRect.width / 2f, 22f, 105f+dotRect.width*1.2f);
			return true;
		}
		return false;
	}

	private bool handleLeft(float x, float y){
		Vector2 pos = new Vector2(x, Screen.height - y);
		if( leftRect.Contains(pos) )
			return true;
		return false;
	}
		
	private bool handleRight(float x, float y){
		Vector2 pos = new Vector2(x, Screen.height - y);
		if( rightRect.Contains(pos) )
			return true;
		return false;
	}

	private bool handleSwop(float x, float y){
		Vector2 pos = new Vector2(x, Screen.height - y);
		if( swopRect.Contains(pos) )
			return true;
		return false;
	}

	void Awake () {
		Input.multiTouchEnabled = true;
		mControl = this;
		mIntroColor	= new Color(1,1,1,0);
	}

	void Start () {
		mTime = Time.time;
		readyRect = SupportLib.getIconRect(Icons.Ready);
		goRect = SupportLib.getIconRect(Icons.Go);
		directionRect = SupportLib.getIconRect(Icons.DirectionBtn);
		dotRect = SupportLib.getIconRect(Icons.DirectionDot);
		//swopRect = SupportLib.getIconRect(Icons.SwopBtn);
		swopRect = SupportLib.getIconRect(Icons.SwopNew);
		leftRect = SupportLib.getIconRect(Icons.LeftBtn);
		rightRect = SupportLib.getIconRect(Icons.RightBtn);

		mActiveRect = readyRect;
		mActiveTex = scanningTex;
	}
	
	// Update is called once per frame
	void Update () {
		if(mState == 0){
			mIntroColor.a = Easing.easeOutQuad(fract(Time.time/2f));
		}else if(mState == 1){
			float time = (Time.time - mTime) / 3f;
			mIntroColor.a = Easing.easeWithType(EasingType.EaseInQuad,time);
			if(time >= 1f){
				mState = 2;
				mTime = Time.time;
				mActiveTex = goTex;
				mActiveRect = goRect;
			}
		}else if(mState == 2){
			float t = Time.time - mTime;
			int round = Mathf.FloorToInt(t / 0.1f);
			if(round % 2 == 0)
				mIntroColor.a = 0f;
			else mIntroColor.a = 1f;
			if(t > 1f){
				mState = 3;
			}
		}

		//touch event check
		bool isTouchingDirection = false, isTouchingSwop = false, leftMove = false, rightMove = false;
		if( Input.touchCount > 0 ){
			for( int i = 0; i < Input.touchCount; i++ ){
				Touch touch = Input.GetTouch(i);
				if(useSlider){
					isTouchingDirection = handleTouch(touch.position.x,touch.position.y);
				}else{
					leftMove = handleLeft(touch.position.x,touch.position.y);
					rightMove = handleRight(touch.position.x,touch.position.y);
				}
				isTouchingSwop = handleSwop(touch.position.x,touch.position.y);
			}
		}else if(Input.GetMouseButton(0)){
			var pos = Input.mousePosition;
			if(useSlider){
				isTouchingDirection = handleTouch(pos.x,pos.y);
			}else{
				leftMove = handleLeft(pos.x,pos.y);
				rightMove = handleRight(pos.x,pos.y);
			}
			isTouchingSwop = handleSwop(pos.x,pos.y);
		}

		if(isTouchingSwop != mSwopDown){
			mSwopDown = isTouchingSwop;
		}

		if(useSlider){
			if(!isTouchingDirection){
				dotRect.x = Mathf.Lerp(dotRect.x, directionRect.center.x - dotRect.width/2f, Time.deltaTime * 4f);
				mDirection = 0f;
			}
		}else{
			if(leftMove && rightMove)
				mDirection = 0f;
			else if(rightMove)
				mDirection = 1f;
			else if(leftMove)
				mDirection = -1f;
			else mDirection = 0f;
		}
	}

	void OnGUI(){
		if(mState < 3){
			GUI.color = mIntroColor;
			GUI.DrawTexture(mActiveRect,mActiveTex);
		}else{
			GUI.color = Color.white;
			if(useSlider){
				GUI.DrawTexture(directionRect, directionBtn);
				GUI.DrawTexture(dotRect, directionDot);
			}else{
				GUI.DrawTexture(leftRect,leftBtn);
				GUI.DrawTexture(rightRect,rightBtn);
			}
			GUI.DrawTexture(swopRect, mSwopDown ? swopBtnOff : swopBtnOn);
		}
	}
}
