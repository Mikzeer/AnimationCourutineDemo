using System.Collections.Generic;
using UnityEngine;

namespace PositionerDemo
{
    public class AttackManagerUI : MonoBehaviour
    {
        [SerializeField] private GameObject shieldPrefab = default;

        private Motion SimpleAttackAndDamageMotion(Animator attackerAnimator, Animator damagedAnimator)
        {
            List<Motion> motions = new List<Motion>();
            // PLAY ATTACK ANIMATION
            Motion motionAttack = new AttackMotion(this, attackerAnimator, 1);
            motions.Add(motionAttack);
            if (GameSoundManager.Instance != null)
            {
                if (GameSoundManager.Instance.audioSource != null)
                {
                    // ATTACK SOUND
                    Motion motionAttackSound = new SoundMotion(this, 1, GameSoundManager.Instance.audioSource, GameSoundManager.Instance.audioClips[0], true);
                    motions.Add(motionAttackSound);
                    // DAMAGE SOUND
                    Motion motionDamageSound = new SoundMotion(this, 1, GameSoundManager.Instance.audioSource, GameSoundManager.Instance.audioClips[2], true);
                    motions.Add(motionDamageSound);
                }
            }
            // PLAY DAMAGE ANIMATION
            Motion motionDamage = new DamageMotion(this, damagedAnimator, 1);
            motions.Add(motionDamage);

            CombineMotion combineAttackMotion = new CombineMotion(this, 1, motions);
            return combineAttackMotion;
        }

        private Motion SimpleAttackMotion(Animator attackerAnimator)
        {
            List<Motion> motions = new List<Motion>();
            // PLAY ATTACK ANIMATION
            Motion motionAttack = new AttackMotion(this, attackerAnimator, 1);
            motions.Add(motionAttack);
            if (GameSoundManager.Instance != null)
            {
                if (GameSoundManager.Instance.audioSource != null)
                {
                    // ATTACK SOUND
                    Motion motionAttackSound = new SoundMotion(this, 1, GameSoundManager.Instance.audioSource, GameSoundManager.Instance.audioClips[0], true);
                    motions.Add(motionAttackSound);
                }
            }
            CombineMotion combineAttackMotion = new CombineMotion(this, 1, motions);
            return combineAttackMotion;
        }

        private Motion SimpleDamageMotion(Animator damagedAnimator)
        {
            List<Motion> motions = new List<Motion>();
            if (GameSoundManager.Instance != null)
            {
                if (GameSoundManager.Instance.audioSource != null)
                {
                    // DAMAGE SOUND
                    Motion motionDamageSound = new SoundMotion(this, 1, GameSoundManager.Instance.audioSource, GameSoundManager.Instance.audioClips[2], true);
                    motions.Add(motionDamageSound);
                }
            }
            // PLAY DAMAGE ANIMATION
            Motion motionDamage = new DamageMotion(this, damagedAnimator, 1);
            motions.Add(motionDamage);

            CombineMotion combineAttackMotion = new CombineMotion(this, 1, motions);
            return combineAttackMotion;
        }

        private Motion SimpleAttackToCombineMotion(Animator attackerAnimator, List<Animator> damagedAnimator)
        {
            List<Motion> motions = new List<Motion>();
            // PLAY ATTACK ANIMATION
            Motion motionAttack = new AttackMotion(this, attackerAnimator, 1);
            motions.Add(motionAttack);
            if (GameSoundManager.Instance != null)
            {
                if (GameSoundManager.Instance.audioSource != null)
                {
                    // ATTACK SOUND
                    Motion motionAttackSound = new SoundMotion(this, 1, GameSoundManager.Instance.audioSource, GameSoundManager.Instance.audioClips[0], true);
                    motions.Add(motionAttackSound);
                    // DAMAGE SOUND
                    Motion motionDamageSound = new SoundMotion(this, 1, GameSoundManager.Instance.audioSource, GameSoundManager.Instance.audioClips[2], true);
                    motions.Add(motionDamageSound);
                }
            }
            // PLAY DAMAGE ANIMATION FOR ALL THE ENEMIES
            for (int i = 0; i < damagedAnimator.Count; i++)
            {
                Motion motionDamage = new DamageMotion(this, damagedAnimator[i], 1);
                motions.Add(motionDamage);
            }
            CombineMotion combineAttackMotion = new CombineMotion(this, 1, motions);
            return combineAttackMotion;
        }

        private Motion CombineDamageMotion(List<Animator> damagedAnimator)
        {
            List<Motion> motions = new List<Motion>();
            if (GameSoundManager.Instance != null)
            {
                if (GameSoundManager.Instance.audioSource != null)
                {
                    // DAMAGE SOUND
                    Motion motionDamageSound = new SoundMotion(this, 1, GameSoundManager.Instance.audioSource, GameSoundManager.Instance.audioClips[2], true);
                    motions.Add(motionDamageSound);
                }
            }
            // PLAY DAMAGE ANIMATION FOR ALL THE ENEMIES
            for (int i = 0; i < damagedAnimator.Count; i++)
            {
                Motion motionDamage = new DamageMotion(this, damagedAnimator[i], 1);
                motions.Add(motionDamage);
            }
            CombineMotion combineAttackMotion = new CombineMotion(this, 1, motions);
            return combineAttackMotion;
        }

        private Motion AttackWithShieldMotion(Animator attackerAnimator, Animator damagedAnimator)
        {
            List<Motion> motions = new List<Motion>();
            List<Configurable> configureAnimotion = new List<Configurable>();
            // PLAY ATTACK ANIMATION
            Motion motionAttack = new AttackMotion(this, attackerAnimator, 1);
            motions.Add(motionAttack);
            if (GameSoundManager.Instance != null)
            {
                if (GameSoundManager.Instance.audioSource != null)
                {
                    // ATTACK SOUND
                    Motion motionAttackSound = new SoundMotion(this, 1, GameSoundManager.Instance.audioSource, GameSoundManager.Instance.audioClips[0], true);
                    motions.Add(motionAttackSound);
                    // DAMAGE SOUND
                    Motion motionDamageSound = new SoundMotion(this, 1, GameSoundManager.Instance.audioSource, GameSoundManager.Instance.audioClips[2], true);
                    motions.Add(motionDamageSound);
                }
            }

            // CREAMOS EL SHIELD
            GameObject shield = Instantiate(shieldPrefab, damagedAnimator.transform.position, Quaternion.identity);
            shield.SetActive(true);

            // PLAY DAMAGE ANIMATION
            Motion motionDamage = new DamageMotion(this, damagedAnimator, 1);
            motions.Add(motionDamage);

            List<Motion> shieldMotions = new List<Motion>();
            Motion motionShieldDamage = new ShieldMotion(this, shield.GetComponent<Animator>(), 1);
            shieldMotions.Add(motionShieldDamage);
            DestroyGOConfigureAnimotion<Transform, Transform> ShieldDestroyConfigAnimotion 
                = new DestroyGOConfigureAnimotion<Transform, Transform>(shield.transform, 2);
            configureAnimotion.Add(ShieldDestroyConfigAnimotion);
            CombineMotion combineShieldMotion = new CombineMotion(this, 1, shieldMotions, configureAnimotion);
            motions.Add(combineShieldMotion);

            CombineMotion combineAttackMotion = new CombineMotion(this, 1, motions);
            return combineAttackMotion;
        }

        private Motion SimpleDamageWithShieldMotion(Animator damagedAnimator)
        {
            List<Motion> motions = new List<Motion>();
            List<Configurable> configureAnimotion = new List<Configurable>();
            if (GameSoundManager.Instance != null)
            {
                if (GameSoundManager.Instance.audioSource != null)
                {
                    // DAMAGE SOUND
                    Motion motionDamageSound = new SoundMotion(this, 1, GameSoundManager.Instance.audioSource, GameSoundManager.Instance.audioClips[2], true);
                    motions.Add(motionDamageSound);
                }
            }

            // CREAMOS EL SHIELD
            GameObject shield = Instantiate(shieldPrefab, damagedAnimator.transform.position, Quaternion.identity);
            shield.SetActive(true);

            // PLAY DAMAGE ANIMATION
            Motion motionDamage = new DamageMotion(this, damagedAnimator, 1);
            motions.Add(motionDamage);

            List<Motion> shieldMotions = new List<Motion>();
            Motion motionShieldDamage = new ShieldMotion(this, shield.GetComponent<Animator>(), 1);
            shieldMotions.Add(motionShieldDamage);
            DestroyGOConfigureAnimotion<Transform, Transform> ShieldDestroyConfigAnimotion
                = new DestroyGOConfigureAnimotion<Transform, Transform>(shield.transform, 2);
            configureAnimotion.Add(ShieldDestroyConfigAnimotion);
            CombineMotion combineShieldMotion = new CombineMotion(this, 1, shieldMotions, configureAnimotion);
            motions.Add(combineShieldMotion);

            CombineMotion combineAttackMotion = new CombineMotion(this, 1, motions);
            return combineAttackMotion;
        }

        private Motion SimpleAttackToCombineWithShieldMotion(Animator attackerAnimator, List<Animator> damagedAnimator)
        {
            List<Motion> motions = new List<Motion>();
            List<Configurable> configureAnimotion = new List<Configurable>();
            // PLAY ATTACK ANIMATION
            Motion motionAttack = new AttackMotion(this, attackerAnimator, 1);
            motions.Add(motionAttack);
            if (GameSoundManager.Instance != null)
            {
                if (GameSoundManager.Instance.audioSource != null)
                {
                    // ATTACK SOUND
                    Motion motionAttackSound = new SoundMotion(this, 1, GameSoundManager.Instance.audioSource, GameSoundManager.Instance.audioClips[0], true);
                    motions.Add(motionAttackSound);
                    // DAMAGE SOUND
                    Motion motionDamageSound = new SoundMotion(this, 1, GameSoundManager.Instance.audioSource, GameSoundManager.Instance.audioClips[2], true);
                    motions.Add(motionDamageSound);
                }
            }
            // PLAY DAMAGE ANIMATION FOR ALL THE ENEMIES
            for (int i = 0; i < damagedAnimator.Count; i++)
            {
                GameObject shield = Instantiate(shieldPrefab, damagedAnimator[i].transform.position, Quaternion.identity);
                shield.SetActive(true);

                Motion motionDamage = new DamageMotion(this, damagedAnimator[i], 1);
                motions.Add(motionDamage);

                List<Motion> shieldMotions = new List<Motion>();
                Motion motionShieldDamage = new ShieldMotion(this, shield.GetComponent<Animator>(), 1);
                shieldMotions.Add(motionShieldDamage);
                DestroyGOConfigureAnimotion<Transform, Transform> ShieldDestroyConfigAnimotion 
                    = new DestroyGOConfigureAnimotion<Transform, Transform>(shield.transform, 2);
                configureAnimotion.Add(ShieldDestroyConfigAnimotion);

                CombineMotion combineShieldMotion = new CombineMotion(this, 1, shieldMotions, configureAnimotion);

                motions.Add(combineShieldMotion);
            }

            CombineMotion combineAttackMotion = new CombineMotion(this, 1, motions);
            return combineAttackMotion;
        }
    }
}
