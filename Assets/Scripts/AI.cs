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

            List<CardController> cardsList = cards.FindAll(x => GameManager.Instance.Enemy.GetMana() >= x.Card.ManaCost);

            if (cardsList.Count == 0)
                break;

            //Использование заклинаний
            if (cardsList[0].Card.IsSpell)
            {
                CastSpell(cardsList[0]);
                yield return new WaitForSeconds(.51f);
            }
            else
            {
                //Выставление карт на стол
                cardsList[0].GetComponent<CardMovement>().MoveToField(GameManager.Instance.EnemyField);
                yield return new WaitForSeconds(.51f);
                cardsList[0].transform.SetParent(GameManager.Instance.EnemyField);
                cardsList[0].OnCast();
            }
        }

        // Для плавности анимаций
        yield return new WaitForSeconds(1);

        while (GameManager.Instance.EnemyFieldCards.Exists(x => x.Card.CanAttack))
        {
            var activeCard = GameManager.Instance.EnemyFieldCards.FindAll(x => x.Card.CanAttack)[0];
            bool hasProvocation = GameManager.Instance.PlayerFieldCards.Exists(x => x.Card.IsProvocation);
            // Если есть провокация бъём именно эту карту 
            if (hasProvocation ||
                Random.Range(0, 2) == 0 &&
                GameManager.Instance.PlayerFieldCards.Count > 0)
            {
                CardController enemy;

                if (hasProvocation)
                    enemy = GameManager.Instance.PlayerFieldCards.Find(x => x.Card.IsProvocation);
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
        switch (((SpellCard)card.Card).SpellTarget)
        {
            case TargetType.NO_TARGET:

                switch (((SpellCard)card.Card).Spell)
                {
                    case SpellType.HEAL_ALLY_FIELD_CARDS:

                        if (GameManager.Instance.EnemyFieldCards.Count > 0)
                            StartCoroutine(CastCard(card));

                        break;

                    case SpellType.DAMAGE_ENEMY_FIELD_CARDS:

                        if (GameManager.Instance.PlayerFieldCards.Count > 0)
                            StartCoroutine(CastCard(card));

                        break;

                    case SpellType.HEAL_ALLY_HERO:
                        StartCoroutine(CastCard(card));
                        break;

                    case SpellType.DAMAGE_ENEMY_HERO:
                        StartCoroutine(CastCard(card));
                        break;
                }

                break;

            case TargetType.ALLY_CARD_TARGET:

                if (GameManager.Instance.EnemyFieldCards.Count > 0)
                    StartCoroutine(CastCard(card,
                        GameManager.Instance.EnemyFieldCards[Random.Range(0, GameManager.Instance.EnemyFieldCards.Count)]));

                break;

            case TargetType.ENEMY_CARD_TARGET:

                if (GameManager.Instance.PlayerFieldCards.Count > 0)
                    StartCoroutine(CastCard(card,
                        GameManager.Instance.PlayerFieldCards[Random.Range(0, GameManager.Instance.PlayerFieldCards.Count)]));

                break;
        }
    }

    //Способности при выставлений карты на стол
    IEnumerator CastCard(CardController spell, CardController target = null)
    {
        //Заклинания
        if (((SpellCard)spell.Card).SpellTarget == TargetType.NO_TARGET)
        {
            spell.GetComponent<CardMovement>().MoveToField(GameManager.Instance.EnemyField);
            yield return new WaitForSeconds(.51f);

            spell.OnCast();
        }
        else
        {
            //Показать карту игроку
            spell.Info.ShowCardInfo();
            spell.GetComponent<CardMovement>().MoveToTarget(target.transform);
            yield return new WaitForSeconds(.51f);

            //Удаляем из руки противника и добавляем в поле противника, также отнимаем ману и противника
            GameManager.Instance.EnemyHandCards.Remove(spell);
            GameManager.Instance.EnemyFieldCards.Add(spell);
            GameManager.Instance.Enemy.ReduceMana(spell.Card.ManaCost);

            spell.Card.IsPlaced = true;
            //Юзаем заклинания
            spell.UseSpell(target);
        }
    }
}