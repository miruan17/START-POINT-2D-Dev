using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Analytics;
public enum StatId_Effect_Lightning { LN, LN_Chain, LN_Range } // dmg, tick, max_stack, term
public class Effect_Lightning : Effect   //Manager class
{
    private Dictionary<StatId_Effect_Lightning, Status> _stats = new();
    private int level = 0;
    public Effect_Lightning(float dmg, int chain, float range)
    {
        chance = 1f;
        _stats[StatId_Effect_Lightning.LN] = new Status(dmg);
        _stats[StatId_Effect_Lightning.LN_Chain] = new Status(chain);
        _stats[StatId_Effect_Lightning.LN_Range] = new Status(range);
    }

    public override void Runtime()
    {
        if (term <= 0) return;

        var caster = manager.GetCharacter();
        Vector3 casterPos = caster.transform.position;

        // 1. Find nearest enemies
        var enemies = Enemy.AllEnemies
            .OrderBy(e => Vector2.Distance(casterPos, e.transform.position));

        float chainCount = _stats[StatId_Effect_Lightning.LN_Chain].Get();
        float maxRange = _stats[StatId_Effect_Lightning.LN_Range].Get();
        float baseDmg = _stats[StatId_Effect_Lightning.LN].Get();

        List<Enemy> chainTargets = new List<Enemy>();

        // 2. Pick enemies within range
        foreach (var e in enemies)
        {
            if (chainTargets.Count >= chainCount) break;
            if (Vector2.Distance(casterPos, e.transform.position) > maxRange) break;
            chainTargets.Add(e);
        }

        if (chainTargets.Count == 0)
        {
            term = 0;
            return;
        }

        // 3. Damage splitting
        float dmg = baseDmg * chainCount / chainTargets.Count;

        foreach (var e in chainTargets)
        {
            e.status.CurrentHP -= dmg;
            Debug.Log($"{e.name} takes {dmg} lightning damage.");
        }

        // 4. Draw chain lightning
        Vector3 prevPos = casterPos;

        foreach (var e in chainTargets)
        {
            Vector3 nextPos = e.transform.position;

            // Multi-line lightning effect
            DrawMultiLightning(prevPos, nextPos);

            prevPos = nextPos; // chain link movement
        }

        term = 0;
    }


    // ------------------------------
    // Multi-Line Lightning (3-lines)
    // ------------------------------
    public void DrawMultiLightning(Vector3 start, Vector3 end)
    {
        CreateLightningLine(start, end, 0f, 0.10f, 1.5f);      // main line
        CreateLightningLine(start, end, -0.05f, 0.12f, 0.7f);  // left aura
        CreateLightningLine(start, end, 0.05f, 0.12f, 0.7f);   // right aura
    }


    // ------------------------------
    // Single line with jitter & brightness
    // ------------------------------
    private void CreateLightningLine(Vector3 start, Vector3 end, float offsetX, float lifetime, float brightness)
    {
        GameObject obj = GameObject.Instantiate(manager.GetCharacter().player.lightning);
        LineRenderer lr = obj.GetComponent<LineRenderer>();

        // Thickness (production settings)
        lr.startWidth = 0.06f;
        lr.endWidth = 0.03f;

        // Color + brightness (URP additive)
        lr.material.SetColor("_BaseColor", new Color(brightness, brightness, brightness * 1.5f, 1));

        // Offset & jitter
        Vector3 offset = new Vector3(offsetX, 0, 0);
        Vector3 jitter = new Vector3(
            UnityEngine.Random.Range(-0.03f, 0.03f),
            UnityEngine.Random.Range(-0.03f, 0.03f),
            0
        );

        lr.positionCount = 2;
        lr.SetPosition(0, start + offset);
        lr.SetPosition(1, end + offset + jitter);

        GameObject.Destroy(obj, lifetime);
    }


    public override void Refresh(Effect effect)
    {
        base.Refresh(effect);
        Effect_Lightning eff = (Effect_Lightning)effect;
        _stats = eff._stats; // 해당 구조를 통해 모든 Effect_Bleeding객체는 항상 player의 EffectLib속 Effect_Bleeding 객체의 _stat을 참조
        level = eff.level;
        chance = effect.chance;
        term = 1;
    }
    public override void updateValue(float term, float dmg, float tick, int max_stack) // 기존 base를 변경하는 연산
    {
        _stats[StatId_Effect_Lightning.LN].SetDefaultValue(_stats[StatId_Effect_Lightning.LN].getBase() + dmg);
    }
    public override void upgrade()
    {
        //호출 횟수(level)에 따른 값 변화. 레벨 많아야 3~4개이므로 하드코딩이 나을듯
        level++;
        if (level == 1)
        {
            _stats[StatId_Effect_Lightning.LN].SetDefaultValue(_stats[StatId_Effect_Lightning.LN].getBase() + 2);
            _stats[StatId_Effect_Lightning.LN_Chain].SetDefaultValue(_stats[StatId_Effect_Lightning.LN_Chain].getBase() + 2);
            chance += 0.05f;
        }
        if (level == 2)
        {
        }
        if (level == 3)
        {
        }
    }
    public override Effect copy()
    {
        Effect_Lightning effect = new Effect_Lightning(_stats[StatId_Effect_Lightning.LN].getBase(), (int)_stats[StatId_Effect_Lightning.LN_Chain].getBase(), _stats[StatId_Effect_Lightning.LN_Range].getBase());
        effect.identifier = identifier;
        effect.chance = chance;
        return effect;
    }
}
