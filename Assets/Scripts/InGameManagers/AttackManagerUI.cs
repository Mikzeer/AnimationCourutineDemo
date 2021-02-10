using System.Collections.Generic;
using UnityEngine;
using MikzeerGame.Animotion;
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

        #region ANIMOTION

        private Animotion SimpleAttackAndDamageAnimotion(Animator attackerAnimator, Animator damagedAnimator)
        {
            List<Animotion> motions = new List<Animotion>();
            // PLAY ATTACK ANIMATION
            Animotion motionAttack = new AttackAnimatedAnimotion(this, 1, attackerAnimator);
            motions.Add(motionAttack);
            if (GameSoundManager.Instance != null && GameSoundManager.Instance.audioSource != null)
            {
                AudioSourceParameter audioParameter = new AudioSourceParameter(true);
                // ATTACK SOUND
                Animotion motionAttackSound = new SoundAnimotion(this, 1, GameSoundManager.Instance.audioSource, GameSoundManager.Instance.audioClips[0], audioParameter);
                motions.Add(motionAttackSound);
                // DAMAGE SOUND
                Animotion motionDamageSound = new SoundAnimotion(this, 1, GameSoundManager.Instance.audioSource, GameSoundManager.Instance.audioClips[2], audioParameter);
                motions.Add(motionDamageSound);
            }
            // PLAY DAMAGE ANIMATION
            Animotion motionDamage = new DamageAnimatedAnimotion(this, 1, damagedAnimator);
            motions.Add(motionDamage);

            CombineAnimotion combineAttackMotion = new CombineAnimotion(this, 1, motions);
            return combineAttackMotion;
        }

        private Animotion SimpleAttackAnimotion(Animator attackerAnimator)
        {
            List<Animotion> motions = new List<Animotion>();
            // PLAY ATTACK ANIMATION
            Animotion motionAttack = new AttackAnimatedAnimotion(this, 1, attackerAnimator);
            motions.Add(motionAttack);
            if (GameSoundManager.Instance != null && GameSoundManager.Instance.audioSource != null)
            {
                AudioSourceParameter audioParameter = new AudioSourceParameter(true);
                // ATTACK SOUND
                Animotion motionAttackSound = new SoundAnimotion(this, 1, GameSoundManager.Instance.audioSource, GameSoundManager.Instance.audioClips[0], audioParameter);
                motions.Add(motionAttackSound);

            }
            CombineAnimotion combineAttackMotion = new CombineAnimotion(this, 1, motions);
            return combineAttackMotion;
        }

        private Animotion SimpleDamageAnimotion(Animator damagedAnimator)
        {
            List<Animotion> motions = new List<Animotion>();
            if (GameSoundManager.Instance != null && GameSoundManager.Instance.audioSource != null)
            {
                AudioSourceParameter audioParameter = new AudioSourceParameter(true);
                // DAMAGE SOUND
                Animotion motionDamageSound = new SoundAnimotion(this, 1, GameSoundManager.Instance.audioSource, GameSoundManager.Instance.audioClips[2], audioParameter);
                motions.Add(motionDamageSound);
            }
            // PLAY DAMAGE ANIMATION
            Animotion motionDamage = new DamageAnimatedAnimotion(this, 1, damagedAnimator);
            motions.Add(motionDamage);

            CombineAnimotion combineAttackMotion = new CombineAnimotion(this, 1, motions);
            return combineAttackMotion;
        }

        private Animotion SimpleAttackToCombineAnimotion(Animator attackerAnimator, List<Animator> damagedAnimator)
        {
            List<Animotion> motions = new List<Animotion>();
            // PLAY ATTACK ANIMATION
            Animotion motionAttack = new AttackAnimatedAnimotion(this, 1, attackerAnimator);
            motions.Add(motionAttack);
            if (GameSoundManager.Instance != null && GameSoundManager.Instance.audioSource != null)
            {
                AudioSourceParameter audioParameter = new AudioSourceParameter(true);
                // ATTACK SOUND
                Animotion motionAttackSound = new SoundAnimotion(this, 1, GameSoundManager.Instance.audioSource, GameSoundManager.Instance.audioClips[0], audioParameter);
                motions.Add(motionAttackSound);
                // DAMAGE SOUND
                Animotion motionDamageSound = new SoundAnimotion(this, 1, GameSoundManager.Instance.audioSource, GameSoundManager.Instance.audioClips[2], audioParameter);
                motions.Add(motionDamageSound);
            }

            // PLAY DAMAGE ANIMATION FOR ALL THE ENEMIES
            for (int i = 0; i < damagedAnimator.Count; i++)
            {
                Animotion motionDamage = new DamageAnimatedAnimotion(this, 1, damagedAnimator[i]);
                motions.Add(motionDamage);
            }
            CombineAnimotion combineAttackMotion = new CombineAnimotion(this, 1, motions);
            return combineAttackMotion;
        }

        private Animotion CombineDamageAnimotion(List<Animator> damagedAnimator)
        {
            List<Animotion> motions = new List<Animotion>();
            if (GameSoundManager.Instance != null && GameSoundManager.Instance.audioSource != null)
            {
                AudioSourceParameter audioParameter = new AudioSourceParameter(true);
                // DAMAGE SOUND
                Animotion motionDamageSound = new SoundAnimotion(this, 1, GameSoundManager.Instance.audioSource, GameSoundManager.Instance.audioClips[2], audioParameter);
                motions.Add(motionDamageSound);
            }

            // PLAY DAMAGE ANIMATION FOR ALL THE ENEMIES
            for (int i = 0; i < damagedAnimator.Count; i++)
            {
                Animotion motionDamage = new DamageAnimatedAnimotion(this, 1, damagedAnimator[i]);
                motions.Add(motionDamage);
            }
            CombineAnimotion combineAttackMotion = new CombineAnimotion(this, 1, motions);
            return combineAttackMotion;
        }

        private Animotion AttackWithShieldAnimotion(Animator attackerAnimator, Animator damagedAnimator)
        {
            List<Animotion> motions = new List<Animotion>();
            List<ConfigurableMotion> configureAnimotion = new List<ConfigurableMotion>();
            // PLAY ATTACK ANIMATION
            Animotion motionAttack = new AttackAnimatedAnimotion(this, 1, attackerAnimator);
            motions.Add(motionAttack);
            if (GameSoundManager.Instance != null && GameSoundManager.Instance.audioSource != null)
            {
                AudioSourceParameter audioParameter = new AudioSourceParameter(true);
                // ATTACK SOUND
                Animotion motionAttackSound = new SoundAnimotion(this, 1, GameSoundManager.Instance.audioSource, GameSoundManager.Instance.audioClips[0], audioParameter);
                motions.Add(motionAttackSound);
                // DAMAGE SOUND
                Animotion motionDamageSound = new SoundAnimotion(this, 1, GameSoundManager.Instance.audioSource, GameSoundManager.Instance.audioClips[2], audioParameter);
                motions.Add(motionDamageSound);
            }
            // PLAY DAMAGE ANIMATION
            Animotion motionDamage = new DamageAnimatedAnimotion(this, 1, damagedAnimator);
            motions.Add(motionDamage);

            // CREAMOS EL SHIELD
            GameObject shield = Instantiate(shieldPrefab, damagedAnimator.transform.position, Quaternion.identity);
            shield.SetActive(true);

            List<Animotion> shieldMotions = new List<Animotion>();
            Animotion motionShieldDamage = new ShieldAnimatedAnimotion(this, 1, shield.GetComponent<Animator>());
            shieldMotions.Add(motionShieldDamage);

            GameObjectDestroyConfigureContainer goDestroyContainer = new GameObjectDestroyConfigureContainer(shield);
            ConfigureAnimotion destroyGOConfigAnimotion = new ConfigureAnimotion(goDestroyContainer, 2);
            configureAnimotion.Add(destroyGOConfigAnimotion);

            CombineAnimotion combineShieldMotion = new CombineAnimotion(this, 1, shieldMotions, configureAnimotion);
            motions.Add(combineShieldMotion);

            CombineAnimotion combineAttackMotion = new CombineAnimotion(this, 1, motions);
            return combineAttackMotion;
        }

        private Animotion SimpleDamageWithShieldAnimotion(Animator damagedAnimator)
        {
            List<Animotion> motions = new List<Animotion>();
            List<ConfigurableMotion> configureAnimotion = new List<ConfigurableMotion>();

            if (GameSoundManager.Instance != null && GameSoundManager.Instance.audioSource != null)
            {
                AudioSourceParameter audioParameter = new AudioSourceParameter(true);
                // DAMAGE SOUND
                Animotion motionDamageSound = new SoundAnimotion(this, 1, GameSoundManager.Instance.audioSource, GameSoundManager.Instance.audioClips[2], audioParameter);
                motions.Add(motionDamageSound);
            }
            // PLAY DAMAGE ANIMATION
            Animotion motionDamage = new DamageAnimatedAnimotion(this, 1, damagedAnimator);
            motions.Add(motionDamage);

            // CREAMOS EL SHIELD
            GameObject shield = Instantiate(shieldPrefab, damagedAnimator.transform.position, Quaternion.identity);
            shield.SetActive(true);

            List<Animotion> shieldMotions = new List<Animotion>();
            Animotion motionShieldDamage = new ShieldAnimatedAnimotion(this, 1, shield.GetComponent<Animator>());
            shieldMotions.Add(motionShieldDamage);

            GameObjectDestroyConfigureContainer goDestroyContainer = new GameObjectDestroyConfigureContainer(shield);
            ConfigureAnimotion destroyGOConfigAnimotion = new ConfigureAnimotion(goDestroyContainer, 2);
            configureAnimotion.Add(destroyGOConfigAnimotion);

            CombineAnimotion combineShieldMotion = new CombineAnimotion(this, 1, shieldMotions, configureAnimotion);
            motions.Add(combineShieldMotion);

            CombineAnimotion combineAttackMotion = new CombineAnimotion(this, 1, motions);
            return combineAttackMotion;
        }

        private Animotion SimpleAttackToCombineWithShieldAnimotion(Animator attackerAnimator, List<Animator> damagedAnimator)
        {
            List<Animotion> motions = new List<Animotion>();
            List<ConfigurableMotion> configureAnimotion = new List<ConfigurableMotion>();
            // PLAY ATTACK ANIMATION
            Animotion motionAttack = new AttackAnimatedAnimotion(this, 1, attackerAnimator);
            motions.Add(motionAttack);
            if (GameSoundManager.Instance != null && GameSoundManager.Instance.audioSource != null)
            {
                AudioSourceParameter audioParameter = new AudioSourceParameter(true);
                // ATTACK SOUND
                Animotion motionAttackSound = new SoundAnimotion(this, 1, GameSoundManager.Instance.audioSource, GameSoundManager.Instance.audioClips[0], audioParameter);
                motions.Add(motionAttackSound);
                // DAMAGE SOUND
                Animotion motionDamageSound = new SoundAnimotion(this, 1, GameSoundManager.Instance.audioSource, GameSoundManager.Instance.audioClips[2], audioParameter);
                motions.Add(motionDamageSound);
            }

            // PLAY DAMAGE ANIMATION FOR ALL THE ENEMIES
            for (int i = 0; i < damagedAnimator.Count; i++)
            {
                // PLAY DAMAGE ANIMATION
                Animotion motionDamage = new DamageAnimatedAnimotion(this, 1, damagedAnimator[i]);
                motions.Add(motionDamage);

                // CREAMOS EL SHIELD
                GameObject shield = Instantiate(shieldPrefab, damagedAnimator[i].transform.position, Quaternion.identity);
                shield.SetActive(true);

                List<Animotion> shieldMotions = new List<Animotion>();
                Animotion motionShieldDamage = new ShieldAnimatedAnimotion(this, 1, shield.GetComponent<Animator>());
                shieldMotions.Add(motionShieldDamage);

                GameObjectDestroyConfigureContainer goDestroyContainer = new GameObjectDestroyConfigureContainer(shield);
                ConfigureAnimotion destroyGOConfigAnimotion = new ConfigureAnimotion(goDestroyContainer, 2);
                configureAnimotion.Add(destroyGOConfigAnimotion);

                CombineAnimotion combineShieldMotion = new CombineAnimotion(this, 1, shieldMotions, configureAnimotion);
                motions.Add(combineShieldMotion);
            }

            CombineAnimotion combineAttackMotion = new CombineAnimotion(this, 1, motions);
            return combineAttackMotion;
        }

        #endregion
    }
}
