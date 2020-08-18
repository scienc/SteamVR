using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SpriteRenderAnim : MonoBehaviour
{
    public float animSpeed = 10;  //动画播放速度 默认1秒播放10帧图片
    private float animTimeInterval = 0;  //帧与帧间隔的时间

    public SpriteRenderer animRenderer;//动画载体的渲染器

    public List<Sprite> SpriteArray = new List<Sprite>(); //序列帧数组
    private int frameIndex = 0;  //帧索引
    private int animLength = 0;  //多少帧
    private float animTimer = 0; //动画时间计时器

    public string spriteFilePath = "";

    void Start()
    {
        animTimeInterval = 1 / animSpeed;//得到每一帧的时间间隔
        animLength = SpriteArray.Count; //得到帧数
    }

    // Update is called once per frame
    void Update()
    {
        animTimer += Time.deltaTime;
        if (animTimer > animTimeInterval)
        {
            animTimer -= animTimeInterval;//当计时器减去一个周期的时间
            frameIndex++;//当帧数自增（播放下一帧）
            frameIndex %= animLength;//判断是否到达最大帧数，到了就从新开始  这里是循环播放的
            animRenderer.sprite = SpriteArray[frameIndex]; //替换图片实现动画
        }
    }

    [ContextMenu("getImage")]
    public void getCarImage()
    {
        SpriteArray.Clear();
        string path = Application.dataPath + "/Resources/" + spriteFilePath;
        if (Directory.Exists(path))
        {
            //获取文件信息
            DirectoryInfo direction = new DirectoryInfo(path);

            FileInfo[] files = direction.GetFiles("*", SearchOption.AllDirectories);

            print(files.Length);

            for (int i = 0; i < files.Length; i++)
            {
                //过滤掉临时文件
                if (files[i].Name.EndsWith(".meta"))
                {
                    continue;
                }
                print(files[i].Extension); //这个是扩展名
                //获取不带扩展名的文件名
                string name = Path.GetFileNameWithoutExtension(files[i].ToString());
                print(name);
                // FileInfo.Name是返回带扩展名的名字 
                SpriteArray.Add((Sprite)Resources.Load(spriteFilePath + "/" + name, typeof(Sprite)));
            }
        }
    }
}
