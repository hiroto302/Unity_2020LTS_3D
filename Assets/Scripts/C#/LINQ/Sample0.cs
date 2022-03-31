using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;  // LINQ 使用宣言


/* LINQ to Objects（以後LINQ)
これは、Listや配列など、コレクションに対するフィルターやグルーピング・加工処理を宣言的に記述する仕組み
従来の手続き的な手法に比較して、宣言的に記述することで生産性と品質の双方を向上できる

実際に「intの配列から偶数を取り出し、小さい順に並べ替えられたListへ変換する」コードで比較する
1. 従来の手続き的記述
2. 手続き型の記述

LINQを使いこなすことで生産性と品質のどちらも向上できる。
実際に使用するにあたり、LINQを実際に使いう前におさえたい3つのポイントを記述する。
    - 3.正しい仕様を宣言する
    - 4.ToListなどで、過度にコレクションを生成しない
    - 5.ToListなどで、適切にコレクションを生成する
*/


namespace LINQToObjects
{
    public class Sample0 : MonoBehaviour
    {
        int[] _numbers = { 5, 10, 8, 3, 6, 12 };

        void Start()
        {
            // SortSmallNum0();
            SortSmallNum1();
        }

        // 1.配列から偶数を取り出し、小さい順に並べ替える処理を 従来の手続き的に記述
        void SortSmallNum0()
        {
            List<int> evenNumbers = new List<int>();
            // 上記を省略して宣言できるようになってる!けど Unityじゃ対応していない?
            // List<int> evenNumbers = new();

            foreach (var number in _numbers)
            {
                if( number % 2 == 0 )
                    evenNumbers.Add(number);
            }

            evenNumbers.Sort((x, y) => x - y);
            foreach (var number in evenNumbers)
            {
                Debug.Log(number);
            }
        }

        // 2. LINQ を使用し宣言的に記述
        void SortSmallNum1()
        {
            var evenNumbers =
                _numbers
                    .Where(x => x % 2 == 0)
                    .OrderBy(x => x)
                    .ToList();

            foreach (var number in evenNumbers)
            {
                Debug.Log(number);
            }
        }

#region  3. 正しい宣言をする

        // ユニークなIdをプロパティに持つ、アイテムクラスがあるとする
        public class Item
        {
            public int Id { get; }
            public Item(int id)
            {
                this.Id = id;
            }
        }
        // Idの重複しないItemのList
        List<Item> items = new[] { 5, 10, 8, 3, 6, 12 }
                                .Select(x => new Item(x))
                                .ToList();

        // ここからIdが3(n)のItemを取り出すとした場合 4つの記述で実装できる
        Item GetItem(int n)
        {
            // 1個だけ存在するものを取得する
            // - 条件に該当するものが存在しない場合、例外をスローする
            // - 条件に該当するものが複数存在する場合、例外をスローする
            var item = items.Single(x => x.Id == n);

            // 0または1個存在するものを取得する
            // - 条件に該当するものが存在しない場合、Default値を返す
            // - 条件に該当するものが複数存在する場合、例外をスローする
            var item1 = items.SingleOrDefault(x => x.Id == n);


            // 1個以上存在するものを取得する
            // - 条件に該当するものが存在しない場合、例外をスローする
            // - 条件に該当するものが複数存在する場合、先頭のオブジェクトを返す
            var item2 = items.First(x => x.Id == n);

            // 0個以上存在するものを取得する
            // - 条件に該当するものが存在しない場合、Default値を返す
            // - 条件に該当するものが複数存在する場合、先頭のオブジェクトを返す
            var item3 = items.FirstOrDefault(x => x.Id == n);

            return item;
        }

        // どれを使うのが「正しい」の考える
        // たとえばFirstOrDefaultで取得した場合、今回のケースであれば該当するitemがないとnullが返される。
        // したがって、戻り値がnullではない場合のみ処理すれば「落ちにくい」プログラムが書けそうである。
        // またSingle系の場合、重複する結果があったら例外をスローする仕様となっているため、itemsの最後まで探索する必要がある。
        // 対してFirst系であれば、条件に該当するものが見つかり次第処理を中断できるため、計算量も少なくて済みそうである。
        // では、つねにFirstOrDefaultを利用しておくことが正しいのか？
        // 結論は「背景による」である。

        // もう少し仕様を具体的に見てみる。
        // あるアプリケーションでは、Idの重複しないItemを複数登録します
        // アプリケーションでは登録されたItemの一覧を表示します
        // 一覧から任意の行を選択すると、該当のItemの詳細を閲覧できます
        // 登録されたItemは、削除されません

        // 仕様が前述の通りであった場合、FirstOrDefaultでも期待通りに動作します。
        //  むしろItemの登録系機能に不具合があり、Idの重複したItemが登録されていても落ちず、それなりに動作する。
        // そして動作も早いかもしれない。ではやはりFirstOrDefaultを利用するのが正しいのか？

        // いいえ。むしろ FirstOrDefaultは、もっとも利用してはいけない。 なぜか？

        // それは障害を一時的に隠ぺいしてしまうからである。
        // SingleもしくはSingleOrDefaultで実装されていれば、Idの重複があれば例外が発生し、その時点で登録系機能に不具合が存在することを検知できる。

        // したがって「仕様が前述の通りであった場合」、First系を利用してはいけない。

        // ではSingleとSingleOrDefaultどちらを使うべきなのか？？
        // singleを使うべきです。なぜなら「仕様通り実装されているなら」条件に該当しないということはあり得ない為である。

        // 上記のケースでnullのための条件分岐があることは、コードを無意味に複雑化する。
        // 将来だれかがそのコードを見たときに、「Itemがない場合も存在するのだろうか？」という錯誤さえ生み出してしまう。
#endregion

#region 4. ToListなどで、過度にコレクションを生成しない
        // Listを作ることは、CPUとメモリーの双方のリソースをそれなりに消費してしまう。ダなList化を減らすだけで何倍も速くなったなんてことはよくあるらしい。
#endregion

#region 5. ToListなどで、適切にコレクションを生成する
        // Itemの配列からIdが偶数のものを取り出し、Idを昇順でコンソールへ出力する」処理を実装するとする。を
        // Itemの配列からIdが偶数のものを取り出し、件数と、Id（昇順）をコンソールへ出力する という処理に変更する場合を考える
        // 下記に、単純な実装方法で記述する
        List<Item> items2 = new[] { 5, 10, 8, 3, 6, 12 }
        .Select(x => new Item(x))
        .ToList();

        void DisplyItemInfo()
        {
            var evenItems =
                items2
                .Where(x => x.Id % 2 == 0)
                .OrderBy(x => x.Id);

            Debug.Log("Count:{evenItems.Count()}");
            foreach (var item in evenItems)
            {
                Debug.Log(item.Id);
            }
        }

        // 上記の実装では、Countの表示前と、foreachのループの2か所でWhereが呼び出されてしまう
        // これはLINQのフィルターや選択処理は、遅延実行されることがその理由である
        // 上記のコードではWhereとOrderByが宣言されているが、それらが呼び出されるのはevenItemsのインスタンスが作られるときではなく、それが使われるときである。

        // そのためevenItemsが2回使われる（Countとforeach）と、LINQの処理が2回実行されることになる !!

        // これはCPUやメモリーを浪費する問題と、
        // それぞれの実行の間におおもとのデータソースが変更された場合に、出力された結果が矛盾した状態になることもある
        // そのためには、ToListなどで、適切にコレクションを生成し、スナップショットを確定させる。
        // 今回のケースでは下記の様に、 ToList を追加した実装をする

        void DisplyItemInfo2()
        {
            var evenItems =
                items2
                .Where(x => x.Id % 2 == 0)
                .OrderBy(x => x.Id)
                .ToList();

            Debug.Log("Count:{evenItems.Count()}");
            foreach (var item in evenItems)
            {
                Debug.Log(item.Id);
            }
        }
        // 上記のようにすることで LINQの処理を1回きりに制限できる

#endregion
    }
}