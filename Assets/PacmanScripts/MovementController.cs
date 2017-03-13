using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;
using System;

public enum Direction{
	Right, Left, Up, Down, None
}

public enum BrickType{
	VerticalDown, VerticalUp, Horizontal
}
	
public enum Side {
	Top, Back, Front, Bottom
}

public struct BrickStruct{
	public Transform transform;
	public BrickType type;
}

public class MovementController : MonoBehaviour {
	public bool isEnemy = false;
	public float speed = 0.01f;
    public float JoystickManualCalibrationAngle = 0f;

    //private Direction mDirection = Direction.None;
    private Side mCurrSide = Side.Top, mNextSide = Side.Top, mPrevSide = Side.Back;
	//private BrickType mBrick = BrickType.Horizontal;

	public BrickCollider[] bricks;
	public BoxCollider coreCollider;

	public Vector3 offset;
	public AudioClip swipe, killed, coin;

	private bool mTransfer = false;
	private int mCurrTrack, mNextTrack, mPrevDirec;
	private float mTime, interval;
	private Vector3[] mStartPoints;
	private Vector3[] mEndPoints;
	private AudioSource mAudio;
	private bool mKilled = false;
	private float mKillTime;

	private bool mSwoping = false;
	private int mSwopCount, mTransferCount, mTransferTotal;
	private float mSwopTarget;

	private const int SwopCount = 12;
	private const int TransferShortCount = 12;
	private const int TransferLongCount = 16;
    private SmartARJoystick mController;

    public void colliderWithBrick(BrickType brick){
		//mBrick = brick;
	}

	public void collectCoin(){
		mAudio.PlayOneShot(coin);
	}

	public void triggerKilledAnimation(){
		if(mTransfer || mSwoping) return;
		mKillTime = Time.time;
		mKilled = true;
		mAudio.PlayOneShot(killed);
	}

	void handleHorizontalBrick(BoxCollider brick, int index){
		Vector3 c = brick.transform.localPosition;
		Vector3 start = brick.transform.localScale; 
		Vector3 end = - brick.transform.localScale;
		float[] val = new float[3];
		val[0] = start.x;
		val[1] = start.y;
		val[2] = start.z;
		float length = Mathf.Max(val);
		float shorth = Mathf.Min(val);

		start.x = c.x;
		start.y = c.y - length/2 + shorth/2;
		start.z = c.z;
		end.x   = c.x;
		end.y   = c.y + length/2 - shorth/2;
		end.z   = c.z;

		mStartPoints[index] = start;
		mEndPoints[index] = end;
	}

	void handleVerticalUpBrick(BoxCollider brick, int index){
		Vector3 c = brick.transform.localPosition; 
		Vector3 start = brick.transform.localScale; 
		Vector3 end = - brick.transform.localScale;
		float[] val = new float[3];
		val[0] = start.x;
		val[1] = start.y;
		val[2] = start.z;
		float length = Mathf.Max(val);
		float shorth = Mathf.Min(val);

		start.x = c.x;
		start.y = c.y;
		start.z = c.z - length/2 + 3 * shorth/2;
		end.x   = c.x;
		end.y   = c.y;
		end.z   = c.z + length/2 - shorth/2;

		mStartPoints[index] = start;
		mEndPoints[index] = end;
	}

	void handleVertcalDownBrick(BoxCollider brick, int index){
		Vector3 c = brick.transform.localPosition; 
		Vector3 start = brick.transform.localScale; 
		Vector3 end = - brick.transform.localScale;
		float[] val = new float[3];
		val[0] = start.x;
		val[1] = start.y;
		val[2] = start.z;
		float length = Mathf.Max(val);
		float shorth = Mathf.Min(val);

		start.x = c.x;
		start.y = c.y;
		start.z = c.z - length/2 + 3 * shorth/2;
		end.x   = c.x;
		end.y   = c.y;
		end.z   = c.z + length/2 - shorth/2;

		mStartPoints[index] = end;
		mEndPoints[index] = start;
	}

	void Start () {
        // Get the joystick
        GameObject joyObject = GameObject.Find("JoyStick");
        try{
            mController = joyObject.GetComponent<SmartARJoystick>();
            mController.configureJoystick(true, JoystickManualCalibrationAngle);
            //mController.configureJoystick(false, true, false, true);
        }catch (Exception e){
            Debug.LogError(e, joyObject);
        }


        mCurrTrack = 0;
		mAudio = GetComponent<AudioSource>();
		mStartPoints = new Vector3[bricks.Length];
		mEndPoints = new Vector3[bricks.Length];
		for(int i = 0; i < bricks.Length; i++){
			if(bricks[i].brickType == BrickType.Horizontal)
				handleHorizontalBrick(bricks[i].getCollider(),i);
			else if(bricks[i].brickType == BrickType.VerticalUp)
				handleVerticalUpBrick(bricks[i].getCollider(),i);
			else handleVertcalDownBrick(bricks[i].getCollider(),i);
		}

		if(!isEnemy){
			mCurrTrack = 0;
			transform.localPosition = mStartPoints[0];
			mPrevDirec = 1;
		}else{ 
			transform.localPosition = mEndPoints[bricks.Length-1];
			mCurrTrack = bricks.Length-1;
			mPrevDirec = -1;
		}
	}

	int checkInsideSegment(Vector3 start, Vector3 end, Vector3 point){
		BrickType type = bricks[mCurrTrack].brickType;
		Vector3 delta = point - start;
		Vector3 lengt = end - start;
		//float xx = delta.x / lengt.x;
		float yy = delta.y / lengt.y;
		float zz = delta.z / lengt.z;
		//Debug.Log(xx+","+yy+","+zz+": " + type.ToString());
		if(type == BrickType.Horizontal){
			if(yy < 0) return -1;
			else if(yy > 1) return 1;
		}
		if(type == BrickType.VerticalUp){
			if(zz < 0) return -1;
			else if(zz > 1) return 1;
		}
		if(type == BrickType.VerticalDown){
			if(zz < 0) return -1;
			else if(zz > 1) return 1;
		}
		return 0;
	}

	void flipDirection(){
		//BrickType type = bricks[mCurrTrack].brickType;
		//if(!isEnemy)
		//	mAudio.PlayOneShot(swipe);
		//if(type == BrickType.Horizontal)
		//	transform.RotateAround(transform.position,transform.parent.forward,180f);
		//else transform.RotateAround(transform.position,transform.parent.up,180f);
		transform.GetChild(0).RotateAround(transform.position,transform.up,180f);
	}

	void decideSwopDirection(){
		if(mNextSide == Side.Front && mCurrSide == Side.Top)
			mSwopTarget = -90f;
		else if(mNextSide == Side.Top && mCurrSide == Side.Back)
			mSwopTarget = -90f;
		else if(mNextSide == Side.Bottom && mCurrSide == Side.Front)
			mSwopTarget = -90f;
		else if(mNextSide == Side.Back && mCurrSide == Side.Bottom)
			mSwopTarget = -90f;
		else mSwopTarget = 90f;
	}

	void LateUpdate () {
		if(!mSwoping && !mTransfer && mKilled){
			float t = Time.time - mKillTime;
			if(t < 2.0f){
				int round = Mathf.FloorToInt(t / 0.1f);
				coreCollider.GetComponent<Renderer>().enabled = (round%2==0);
			}else{ 
				mKilled = false;
				coreCollider.GetComponent<Renderer>().enabled = true;
			}
			return;
		}

        var command = mController.getAxisOutput();
        command.x *= -1;
        command.y *= -1;
        var isSwoop = Mathf.Abs(command.y) > 0.5f;

        if (!isEnemy && isSwoop){
			if(!mSwoping && !mTransfer){
				mAudio.PlayOneShot(swipe);
				mSwoping = true;
				mSwopCount = SwopCount; //Time.time;
				if(bricks[mCurrTrack].isGrounded){
                    if (mCurrSide == Side.Top) {
                        if (command.y > 0f){
                            mNextSide = Side.Back;
                        }else{
                            mNextSide = Side.Front;
                        }
					}else if(mCurrSide == Side.Front){
                        if (command.y > 0f){
                            mNextSide = Side.Top;
                        }
                    } else if(mCurrSide == Side.Back){
                        if(command.y < 0f){
                            mNextSide = Side.Top;
                        }
                    }
				}else{
					if(mCurrSide == Side.Top){
						if(command.y < 0f)
							mNextSide = Side.Front;
						else mNextSide = Side.Back;
					}else if(mCurrSide == Side.Front){
						if(command.y < 0f)
							mNextSide = Side.Bottom;
						else mNextSide = Side.Top;
					}else if(mCurrSide == Side.Back){
						if(command.y < 0f)
							mNextSide = Side.Top;
						else mNextSide = Side.Bottom;
					}else if(mCurrSide == Side.Bottom){
						if(command.y < 0f)
							mNextSide = Side.Back;
						else mNextSide = Side.Front;
					}
				}
				decideSwopDirection();
			}
		}

		if(mSwoping){
			//const float swopInterval = 0.2f;
			//float delta = Time.time - mSwopTime;
			if(mSwopCount > 0){
				Vector3 axis;
				if(bricks[mCurrTrack].brickType == BrickType.Horizontal)
					axis = transform.parent.up;
				else axis = transform.parent.forward;
				mSwopCount -= 1;
				//float c = swopInterval / Time.deltaTime;
				transform.RotateAround(transform.position,axis,mSwopTarget/SwopCount);
			}else{ 
				mSwoping = false;
				mPrevSide = mCurrSide;
				mCurrSide = mNextSide;
			}
		}else if(mTransfer){
			float delta = Time.time - mTime;
			if(mTransferCount > 0){
				bool forward  = mNextTrack > mCurrTrack;
				Vector3 start = forward ? mEndPoints[mCurrTrack] : mStartPoints[mCurrTrack];
				Vector3 end   = forward ? mStartPoints[mNextTrack] : mEndPoints[mNextTrack];
				float r = Mathf.Clamp01(delta / interval);

				BrickType nextType = bricks[mNextTrack].brickType;
				BrickType currType = bricks[mCurrTrack].brickType;
				float adder = 0f;
				if(nextType == BrickType.VerticalUp && currType == BrickType.Horizontal)
					adder = 90f;
				else if(nextType == BrickType.Horizontal && currType == BrickType.VerticalUp)
					adder = -90f;
				else if(nextType == BrickType.VerticalDown && currType == BrickType.Horizontal)
					adder = -90f;
				else if(nextType == BrickType.Horizontal && currType == BrickType.VerticalDown)
					adder = 90f;
				mTransferCount -= 1;
				transform.localPosition = Vector3.Lerp(start,end,r);
				transform.RotateAround(transform.position,transform.parent.right,adder/mTransferTotal);
			}else{ 
				mTransfer = false;
				mCurrTrack = mNextTrack;
			}
		}else{
			var direc = 0;
			if(isEnemy){
				direc = mPrevDirec;
			}else{
				direc = command.x > 0.3f ? 1 : (command.x < -0.3f ? -1 : 0);
			}

			if(direc == 0)
				return;
			
			Vector3 start = mStartPoints[mCurrTrack];
			Vector3 end   = mEndPoints[mCurrTrack];
			var forward = (end - start).normalized;
			transform.localPosition += speed * direc * Time.deltaTime * forward;
			var position = transform.localPosition;
			int checker = checkInsideSegment(start,end,position);

			if(!isEnemy){
				if(mPrevDirec * direc < 0)
					flipDirection();
				mPrevDirec = direc;
			}

			if(checker == 0 || checker != direc)
				return;

			if(direc < 0){
				transform.localPosition = start;
				if(mCurrTrack <= 0){ 
					if(isEnemy){
						flipDirection();
						mPrevDirec *= -1;
					}
					return;
				}
				mNextTrack = mCurrTrack -1;
			}else if(direc > 0){
				transform.localPosition = end;
				if(mCurrTrack == bricks.Length-1){ 
					if(isEnemy){
						flipDirection();
						mPrevDirec *= -1;
					}
					return;
				}
				mNextTrack = mCurrTrack + 1;
			}

			if(mCurrSide == Side.Bottom && bricks[mNextTrack].isGrounded){
				mNextTrack = mCurrTrack;
				return;
			}

			if(bricks[mCurrTrack].brickType == BrickType.Horizontal && bricks[mNextTrack].brickType == BrickType.VerticalUp)
				interval = 0.3f;
			else if(bricks[mCurrTrack].brickType == BrickType.VerticalUp && bricks[mNextTrack].brickType == BrickType.Horizontal)
				interval = 0.5f;
			else if(bricks[mCurrTrack].brickType == BrickType.Horizontal && bricks[mNextTrack].brickType == BrickType.VerticalDown)
				interval = 0.5f;
			else if(bricks[mCurrTrack].brickType == BrickType.VerticalDown && bricks[mNextTrack].brickType == BrickType.Horizontal)
				interval = 0.3f;
			mTransfer = true;
			if(interval > 0.3f)
				mTransferCount = TransferLongCount;
			else mTransferCount = TransferShortCount;
			mTransferTotal = mTransferCount;
			mTime = Time.time;
		}
	}
}
