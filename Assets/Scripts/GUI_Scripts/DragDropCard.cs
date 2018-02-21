using Cards;
using UnityEngine;

public class DragDropCard: UIDragDropItem
{
    private UISprite BackgroundSprite;
    private UISprite CardSprite;
    private Card card;
    private SpringPosition sp;
    
    protected override void Start()
    {
        base.Start();
        sp = gameObject.AddComponent<SpringPosition>();
        
        BackgroundSprite = GetComponent<UISprite>();
        CardSprite = GetComponentsInChildren<UISprite>()[1];
        card = GetComponent<Card>();
    }

    protected override void OnDragDropEnd()
    {
        base.OnDragDropEnd();
        if (card.ZoneApplication!=ZoneApplication.Everything)
        {
            if (ApplyZoneCard()) //условие на правильную область применения таргетной карты
            {
                //если правда, то применяем карту
                ApplyCard();
            }
            else //иначе возвращаем карту в руку
            {
                sp.enabled = true;
                if (card.type == CardType.TargetZoneCard)
                    ResetCardSize();
                card.PutInHand();
            }
        }
        else //если она не таргетная, можем применить ее всегда
        {
            ApplyCard();
        }
        
    }

    private void ApplyCard()
    {
        if(card.ApplyEffect(GetWorldCoordinate()))
        {
            CardManager.instance.RemoveCardFromHand(card);
            Destroy(gameObject);
        }
        else
        {
            sp.enabled = true;
            if (card.type == CardType.TargetZoneCard)
                ResetCardSize();
            card.PutInHand();
        }
    }

    private bool ApplyZoneCard()
    {
        if(card.ZoneApplication==ZoneApplication.Nothing &&
           Grid.instance.CloseNode(GetWorldCoordinate()))
                return  true;
        if(card.ZoneApplication==ZoneApplication.OnlyBase && 
           Grid.instance.NodeWithBase(GetWorldCoordinate()))
                return true;       
        if(card.ZoneApplication==ZoneApplication.OnlyTower && 
           Grid.instance.NodeWithTower(GetWorldCoordinate()))
                return  true;
        return false;
    }
      
    protected override void OnDragDropStart()
    {
        base.OnDragDropStart();
        if (card.type == CardType.TargetCard)
            BackgroundSprite.spriteName = "cross";
        if (card.type == CardType.TargetZoneCard)
        {
            DrawZoneCard((TargetZoneCard) card);
        }
        if(card.type!=CardType.NoneTargetCard)
            CardSprite.enabled = false;
        card.CardInField();
    }

    protected override void OnDragDropRelease(GameObject surface)
    {
        BackgroundSprite.spriteName = "descriptionLabel";
        base.OnDragDropRelease(surface);
        CardSprite.enabled = true;
    }

    public Vector2 GetWorldCoordinate()
    {
        Vector2 worldPos = UICamera.currentCamera.WorldToViewportPoint(mTrans.position);
        worldPos = Camera.main.ViewportToWorldPoint(worldPos);
        return worldPos;       
    }

    private void DrawZoneCard(TargetZoneCard card)
    {
        BackgroundSprite.spriteName = "Zone";
        BackgroundSprite.width = BackgroundSprite.height =(int)((card.Radius)*(UIRoot.list[0].manualHeight / Camera.main.orthographicSize));
    }

    private void ResetCardSize()
    {
        BackgroundSprite.height = 100;
        BackgroundSprite.width = 128;
    }
}