using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class ItemPillar : Pillar
{
    public enum ItemType
    {
        AddCombo,
        MaintainCombo,
        Guard,
        PushDragon
    }

    public abstract class Item
    {
        public int Count;
        public abstract ItemType Type { get; }
        public abstract void Use();
    }

    public class AddComboItem : Item
    {
        public override ItemType Type => ItemType.AddCombo;

        public override void Use()
        {
            int random = Random.Range(5, 10);
            ComboManager.Instance.AddCombo(random);
            MultiLogManager.Instance.Log($"<color=#00ffff>������ ���: �޺� �߰� - x{random}</color>");
            Debug.Log($"�޺� �߰�");
        }

        public AddComboItem()
        {
            Count = 1;
        }
    }

    public class MaintainComboItem : Item
    {
        public override ItemType Type => ItemType.MaintainCombo;

        public override void Use()
        {
            ComboManager.Instance.FreezeCombo(5f);
            MultiLogManager.Instance.Log($"<color=#00ffff>������ ���: �޺� ���� - 5��</color>");
            Debug.Log($"�޺� ����");
        }

        public MaintainComboItem()
        {
            Count = 1;
        }
    }

    public class GuardItem : Item
    {
        public override ItemType Type => ItemType.Guard;

        public override void Use()
        {
            PlayerController.Instance.AddGuard();
            MultiLogManager.Instance.Log($"<color=#00ffff>������ ���: ���� - ���� ���� ���� ���</color>");
            Debug.Log($"�� ���");
        }

        public GuardItem()
        {
            Count = 1;
        }
    }

    public class FreezeDragonItem : Item
    {
        public override ItemType Type => ItemType.PushDragon;

        public override void Use()
        {
            if (ChaserGenerator.Instance.Chaser != null)
                ChaserGenerator.Instance.Chaser.Freeze(5);
            MultiLogManager.Instance.Log($"<color=#00ffff>������ ���: �巡�� ���� - 5��</color>");
            Debug.Log($"�巡�� ����");
        }

        public FreezeDragonItem()
        {
            Count = 1;
        }
    }

    public static Item[] Items = new Item[]
    {
        new AddComboItem(),
        new MaintainComboItem(),
        new GuardItem(),
        new FreezeDragonItem()
    };

    [SerializeField] SpriteRenderer _icon = null;

    public override void TowerEvent()
    {
        SoundManager.Instance.PlaySound(Effect.GetItem);
        float random = Random.value;
        if(random<=0.35f)
        {
            Items[0].Use();
        }
        else if(random>0.35f&&random<=0.7f)
        {
            Items[1].Use();
        }
        else if(random>0.7f&&random<=0.9f)
        {
            Items[2].Use();
        }
        else
        {
            Items[3].Use();
        }
        /*for (int i = 0; i < Items.Length; i++)
        {
            if (random < Items[i].Count)
            {
                Items[i].Use();
                break;
            }
            else
            {
                random -= Items[i].Count;
            }
        }*/
    }

    public override void Initialize()
    {
        base.Initialize();
        _icon.DOFade(1, 0.2f).From();
    }
}
