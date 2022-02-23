using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEngine.UI;

/* Model - View - (Reactive) Presenter パターン について
MV(R)P パターンと呼ばれる UniRx を活用した UI の構築パターンである。

Model(データ本体を保持するオブジェクト) と View(UIなどの表示機構)を、Presenter と呼ばれるオブジェクトを仲介して
繋ぐでサインパターンのことである。

この、Model と View 間のイベントのやりとりに UniRx を活用したものを作成していく。
UniRx を繋ぎ込みで利用することの利点は、 Model と View を 「過疎結合」に出来る点である。

Model 側の実装を ReactiveProperty などにする必要があるが、それを行うだけで Model と View の繋ぎ込みがスムーズになる。

今回、「Up / Down ボタンを押す毎に、値が 加算 / 減算 され UI にその値が反映される」という仕組みを実装する
*/

namespace UniRxSample.MVRP
{
    // Model と View(uGUI) を繋ぐオブジェクト
    public class Presenter : MonoBehaviour
    {
        // Model オブジェクト
        [SerializeField] Model _model;

        // View オブジェクト (各UI 要素)
        [SerializeField] Text _text;
        [SerializeField] Button _upButton;
        [SerializeField] Button _downButton;

        void Start()
        {
            // Model の値の変化を Text に反映
            _model.Score
                .Subscribe( x => _text.text = x.ToString())
                .AddTo(this);

            // Up が押されたら 加算
            _upButton.OnClickAsObservable()
                .Subscribe(_ => _model.Score.Value++)
                .AddTo(this);

            // Down が押されたら減算
            _downButton.OnClickAsObservable()
                .Subscribe(_ => _model.Score.Value--)
                .AddTo(this);
        }

    }
}
