using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;
using System.Collections.Generic;

namespace DotNetCore_Redis_Demo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CacheController : ControllerBase
    {
        //using StackExchange.Redis;
        private readonly IDatabase _database;

        // 在類別初始化建構子 就先建立Redis預設資料
        public CacheController(IDatabase database)
        {
            _database = database;

            // 加入一筆資料 Welcome(Key):Hello World(Value)
            _database.StringSet("Welcome", "Hello World");

            // 加入一筆資料 Author(Key):Hello Lee(Value)
            _database.StringSet("Author", "Lee");
        }

        // GET: api/Cache?key=key
        // Get 參數最好都要有宣告[FromQuery]去包住，要有明確定義參數類型
        /// <summary>
        /// 在Redis查詢取得該筆Key的值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HttpGet]
        public string Get([FromQuery] string key = "Welcome")
        {
            return _database.StringGet(key);
        }

        // using System.Collections.Generic;
        // POST: api/Cache
        // Post 參數最好都要有宣告[FromBody]去包住，[FromBody]對應Form表單或JSON(raw)，要有明確定義參數類型
        /// <summary>
        /// 在Redis新增一筆紀錄
        /// </summary>
        /// <param name="keyValue"></param>
        [HttpPost]
        public void Post([FromBody] KeyValuePair<string, string> keyValue)
        {
            // 在Redis是用欄位(Key) = 欄位值(Value)作為資料結構
            _database.StringSet(keyValue.Key, keyValue.Value);
        }
    }
}