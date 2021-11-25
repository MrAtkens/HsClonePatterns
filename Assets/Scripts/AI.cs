using System.Collections;
using System.Collections.Generic;
using Models;
using Models.Enums;
using UnityEngine;

public class AI : MonoBehaviour
{
    public void MakeTurn()
    {
        StartCoroutine(EnemyTurn(GameManager.Instance.EnemyHandCards));
    }

    IEnumerator EnemyTurn(List<CardController> cards)
    {
        yield return new WaitForSeconds(1);

        int count = cards.Count == 1 ? 1 :
            Random.Range(0, cards.Count);

        for (int i = 0; i < count; i++)
        {
            //Если противник нечего не может сделать
            if (GameManager.Instance.EnemyFieldCards.Count > 5 ||
                GameManager.Instance.Enemy.GetMana() == 0 ||
                GameManager.Instance.EnemyHandCards.Count == 0)
                break;

            List<CardController> cardsList = cards.FindAll(x => GameManager.Instance.Enemy.GetMana() >= x.Card.ManaCost.Value);

            if (cardsList.Count == 0)
                break;

            //Использование заклинаний
            if (cardsList[i].Card.IsSpell())
            {
                CastSpell(cardsList[i]);
                yield return new WaitForSeconds(.51f);
            }
            else
            {
                //Выставление карт на стол
                cardsList[i].GetComponent<CardMovement>().MoveToField(GameManager.Instance.EnemyField);
                yield return new WaitForSeconds(.51f);
                cardsList[i].transform.SetParent(GameManager.Instance.EnemyField);
                cardsList[i].OnCast();
            }
        }

        // Для плавности анимаций
        yield return new WaitForSeconds(1);

        while (GameManager.Instance.EnemyFieldCards.Exists(x => x.Card.CanAttack))
        {
            var activeCard = GameManager.Instance.EnemyFieldCards.FindAll(x => x.Card.CanAttack)[0];
            bool hasProvocation = GameManager.Instance.PlayerFieldCards.Exists(x => x.IsProvocation());
            // Если есть провокация бъём именно эту карту 
            if (hasProvocation ||
                Random.Range(0, 2) == 0 &&
                GameManager.Instance.PlayerFieldCards.Count > 0)
            {
                CardController enemy;
                if (hasProvocation)
                    enemy = GameManager.Instance.PlayerFieldCards.Find(x => x.IsProvocation());
                else
                    enemy = GameManager.Instance.PlayerFieldCards[Random.Range(0, GameManager.Instance.PlayerFieldCards.Count)];
                
                activeCard.Movement.MoveToTarget(enemy.transform);
                yield return new WaitForSeconds(.75f);

                GameManager.Instance.CardsFight(activeCard, enemy);
            }
            // Бъём лицо противника
            else
            {
                activeCard.GetComponent<CardMovement>().MoveToTarget(GameManager.Instance.Player.transform);
                yield return new WaitForSeconds(.75f);

                GameManager.Instance.DamageHero(activeCard, false);
            }

            yield return new WaitForSeconds(.2f);
        }

        yield return new WaitForSeconds(1);
        GameManager.Instance.ChangeTurn();
    }

    void CastSpell(CardController card)
    {
        //Работа заклинаний на ходу противника
        switch (card.Card.GetTargetType())
        {
            case (int)TargetType.NO_TARGET:

                switch (card.Card.GetAbility()[0])
                {
                    case (int)SpellType.HEAL_ALLY_FIELD_CARDS:

                        if (GameManager.Instance.EnemyFieldCards.Count > 0)
                            StartCoroutine(CastCard(card));

                        break;

                    case (int)SpellType.DAMAGE_ENEMY_FIELD_CARDS:

                        if (GameManager.Instance.PlayerFieldCards.Count > 0)
                            StartCoroutine(CastCard(card));

                        break;

                    case (int)SpellType.HEAL_ALLY_HERO:
                        StartCoroutine(CastCard(card));
                        break;

                    case (int)SpellType.DAMAGE_ENEMY_HERO:
                        StartCoroutine(CastCard(card));
                        break;
                }

                break;

            case (int)TargetType.ALLY_CARD_TARGET:

                if (GameManager.Instance.EnemyFieldCards.Count > 0)
                    StartCoroutine(CastCard(card,
                        GameManager.Instance.EnemyFieldCards[Random.Range(0, GameManager.Instance.EnemyFieldCards.Count)]));

                break;

            case (int)TargetType.ENEMY_CARD_TARGET:

                if (GameManager.Instance.PlayerFieldCards.Count > 0)
                    StartCoroutine(CastCard(card,
                        GameManager.Instance.PlayerFieldCards[Random.Range(0, GameManager.Instance.PlayerFieldCards.Count)]));

                break;
        }
    }

    //Способности при выставлений карты на стол
    IEnumerator CastCard(CardController spell, CardController target = null)
    {
        if (spell.Card.GetTargetType() == (int) TargetType.NO_TARGET)
        {
            spell.GetComponent<CardMovement>().MoveToField(GameManager.Instance.EnemyField);
            yield return new WaitForSeconds(.51f);

            spell.OnCast();
        }
        else
        {
            spell.Info.ShowCardInfo();
            spell.GetComponent<CardMovement>().MoveToTarget(target.transform);
            yield return new WaitForSeconds(.51f);

            GameManager.Instance.EnemyHandCards.Remove(spell);
            GameManager.Instance.EnemyFieldCards.Add(spell);
            GameManager.Instance.Enemy.ReduceMana(spell.Card.ManaCost.Value);

            spell.Card.IsPlaced = true;

            spell.UseSpell(target);
        }

        string targetStr = target == null ? "no_target" : target.Card.Name;
        Debug.Log("AI spell cast: " + (spell.Card).Name + " target: " + targetStr);
    }
}